using YouAreHeard.Enums;
using YouAreHeard.Models;
using YouAreHeard.NewFolder;
using YouAreHeard.Repositories.Interfaces;
using YouAreHeard.Services.Interfaces;

namespace YouAreHeard.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IZoomService _zoomService;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IPayOSService _payOSService;

        public AppointmentService(
            IAppointmentRepository appointmentRepository,
            IZoomService zoomService,
            IScheduleRepository scheduleRepository,
            IDoctorRepository doctorRepository,
            IPayOSService payOSService)
        {
            _appointmentRepository = appointmentRepository;
            _zoomService = zoomService;
            _scheduleRepository = scheduleRepository;
            _doctorRepository = doctorRepository;
            _payOSService = payOSService;
        }

        public string RequestAppointmentAsync(RequestAppointmentDTO requestAppointment)
        {
            // Lazy clean-up
            CleanUpAllSchedulesAvailability();

            // Validate the schedule
            var schedule = _scheduleRepository.GetScheduleById(requestAppointment.DoctorScheduleID, DateTime.Now);
            if (schedule == null)
            {
                throw new Exception("Schedule not found.");
            }

            if (schedule.DoctorScheduleID == DoctorScheduleStatusEnum.Close)
            {
                throw new Exception("Schedule is full. ");
            }

            // Validate the queue number
            int currentQueue = _appointmentRepository.GetQueueCountByScheduleId(requestAppointment.DoctorScheduleID, new List<int>()
            {
                AppointmentStatusEnum.Confirmed,
                AppointmentStatusEnum.Pending
            });
            if (currentQueue >= Constraints.AmountOfPersonPerSchedule)
            {
                throw new Exception("Maximum number of appointments reached.");
            }

            // Update queue number
            int queueNumber = currentQueue + 1;

            // Set appointment status as pending state
            var appointmentDTO = new AppointmentDTO
            {
                IsOnline = requestAppointment.IsOnline,
                DoctorScheduleID = requestAppointment.DoctorScheduleID,
                IsAnonymous = requestAppointment.IsAnonymous,
                PatientID = requestAppointment.PatientID,
                DoctorID = requestAppointment.DoctorID,
                Notes = requestAppointment.Notes,
                Reason = requestAppointment.Reason,
                QueueNumber = queueNumber,
                StartTime = schedule.StartTime,
                ZoomLink = null,
                EndTime = schedule.EndTime,
                ScheduleDate = schedule.Date,
                AppointmentStatusID = AppointmentStatusEnum.Pending,
                CreatedDate = DateTime.Now
            };

            int orderCode = GenerateUniqueOrderCode(appointmentDTO.AppointmentID);
            appointmentDTO.OrderCode = orderCode.ToString();

            int appointmentId = _appointmentRepository.InsertAppointment(appointmentDTO);
            if (queueNumber >= Constraints.AmountOfPersonPerSchedule)
            {
                _scheduleRepository.UpdateScheduleStatus(appointmentDTO.DoctorScheduleID, DoctorScheduleStatusEnum.Close);
            }

            AppointmentDTO appointment2 = _appointmentRepository.GetAppointmentById(appointmentId);

            string paymentUrl = _payOSService.GeneratePaymentUrl(new PayOSPaymentRequest
            {
                OrderCode = orderCode,
                Amount = 30000,
                Description = $"Thanh toán cuộc hẹn #{appointmentId}",
                BuyerName = appointment2.PatientName,
                BuyerEmail = "test@gmail.com",
                BuyerPhone = appointment2.PatientPhone,
                BuyerAddress = "Hanoi, Vietnam",
                Items = new List<object>()
            });

            return paymentUrl;
        }

        public List<AppointmentDTO> GetAppointmentsByPatientId(int patientId)
        {
            return _appointmentRepository.GetAppointmentsByPatientId(patientId, AppointmentStatusEnum.Confirmed);
        }

        public List<AppointmentDTO> GetAppointmentsByDoctorId(int doctorId)
        {
            var appointments = _appointmentRepository.GetAppointmentsByDoctorId(doctorId, AppointmentStatusEnum.Confirmed);

            foreach (var appointment in appointments)
            {
                if (appointment.IsAnonymous)
                {
                    appointment.PatientName = ConstraintWords.AnonymousName;
                    appointment.PatientPhone = ConstraintWords.AnonymousName;
                }
            }

            return appointments;
        }

        public void CancelAppointmentById(int appointmentId)
        {
            // Mark the appointment as cancelled
            _appointmentRepository.UpdateAppointmentStatus(appointmentId, AppointmentStatusEnum.Cancelled);

            // Retrieve the appointment info
            var appointment = _appointmentRepository.GetAppointmentById(appointmentId);
            int scheduleId = appointment.DoctorScheduleID;

            // Adjust other queue numbers
            AdjustQueueAfterCancellation(scheduleId, appointment.QueueNumber);

            // Reopen the schedule if under max capacity
            int remaining = _appointmentRepository.GetQueueCountByScheduleId(scheduleId, new List<int>() {
                AppointmentStatusEnum.Confirmed, AppointmentStatusEnum.Pending
            });
            if (remaining == 0)
            {
                _scheduleRepository.UpdateScheduleStatus(scheduleId, DoctorScheduleStatusEnum.Open);
            }
            else if (remaining < Constraints.AmountOfPersonPerSchedule)
            {
                _scheduleRepository.UpdateScheduleStatus(scheduleId, DoctorScheduleStatusEnum.Pending);
            }
        }

        public AppointmentDTO GetAppointmentWithPatientDetailsById(int appointmentId)
        {
            var appointment = _appointmentRepository.GetAppointmentWithPatientDetailsById(appointmentId);
            if (appointment.IsAnonymous)
            {
                appointment.PatientName = ConstraintWords.AnonymousName;
                appointment.PatientPhone = ConstraintWords.AnonymousName;
            }
            return appointment;
        }

        public async Task<AppointmentDTO> HandlePayOSWebhookAsync(string orderCode)
        {
            var appointment = _appointmentRepository.GetAppointmentByOrderCode(orderCode);
            if (appointment == null)
                throw new Exception("Appointment not found.");

            if (appointment.AppointmentStatusID == AppointmentStatusEnum.Confirmed)
                return appointment;

            int currentConfirmed = _appointmentRepository.GetQueueCountByScheduleId(
                appointment.DoctorScheduleID, new List<int>()
                {
                    AppointmentStatusEnum.Confirmed, AppointmentStatusEnum.Pending
                });
            if (currentConfirmed >= Constraints.AmountOfPersonPerSchedule)
                throw new Exception("Appointment limit reached before payment was confirmed.");

            _appointmentRepository.UpdateAppointmentStatus(appointment.AppointmentID, AppointmentStatusEnum.Confirmed);

            if (appointment.IsOnline && string.IsNullOrEmpty(appointment.ZoomLink))
            {
                appointment.ZoomLink = await _zoomService.GenerateZoomLink(appointment);
                _appointmentRepository.UpdateZoomLink(appointment.AppointmentID, appointment.ZoomLink);
            }

            var doctorProfile = _doctorRepository.GetDoctorProfileByDoctorId(appointment.DoctorID);
            appointment.DoctorName = doctorProfile.Name;

            return appointment;
        }

        private void AdjustQueueAfterCancellation(int doctorScheduleId, int? canceledQueue)
        {
            if (canceledQueue == null)
                return;

            var appointments = _appointmentRepository.GetConfirmedAppointmentsByScheduleId(doctorScheduleId);

            foreach (var appointment in appointments)
            {
                if (appointment.QueueNumber.HasValue && appointment.QueueNumber > canceledQueue)
                {
                    _appointmentRepository.UpdateQueueNumber(appointment.AppointmentID, appointment.QueueNumber.Value - 1);
                }
            }
        }

        private int GenerateUniqueOrderCode(int appointmentId)
        {
            long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            string input = $"{appointmentId}_{timestamp}";

            int hash = Math.Abs(input.GetHashCode());

            return hash % 1_000_000_000;
        }

        private void CleanUpAndRefreshScheduleAvailability(DoctorScheduleDTO doctorSchedule)
        {
            // 1. Clean expired pending appointments
            _appointmentRepository.CancelExpiredPendingAppointmentsBySchedule(
                doctorSchedule.DoctorScheduleID,
                DateTime.Now,
                Constraints.ExpiredPendingAppointment
            );

            // 2. Count current queue (Confirmed + Pending)
            int currentQueue = _appointmentRepository.GetQueueCountByScheduleId(doctorSchedule.DoctorScheduleID, new List<int>
            {
                AppointmentStatusEnum.Confirmed,
                AppointmentStatusEnum.Pending
            });

            // 3. Determine if schedule should still be available
            int status = doctorSchedule.DoctorScheduleStatus;
            if (currentQueue == 0)
            {
                status = DoctorScheduleStatusEnum.Open;
            }
            else if (currentQueue < Constraints.AmountOfPersonPerSchedule)
            {
                status = DoctorScheduleStatusEnum.Pending;
            }
            ;


            // 4. Update availability in DB
            _scheduleRepository.UpdateScheduleStatus(doctorSchedule.DoctorScheduleID, status);
        }

        private void CleanUpAllSchedulesAvailability()
        {
            var allSchedules = _scheduleRepository.GetAllSchedules();
            foreach (var schedule in allSchedules)
            {
                CleanUpAndRefreshScheduleAvailability(schedule);
            }
        }
    }
}