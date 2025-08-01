using Azure.Core;
using YouAreHeard.Enums;
using YouAreHeard.Helper;
using YouAreHeard.Models;
using YouAreHeard.NewFolder;
using YouAreHeard.Repositories.Interfaces;
using YouAreHeard.Services.Interfaces;
using YouAreHeard.Utilities;

namespace YouAreHeard.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IZoomService _zoomService;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IPayOSService _payOSService;
        private readonly IDoctorService _doctorService;
        private readonly IAppointmentStatusRepository _appointmentStatusRepository;

        public AppointmentService(
            IAppointmentRepository appointmentRepository,
            IZoomService zoomService,
            IScheduleRepository scheduleRepository,
            IDoctorRepository doctorRepository,
            IPayOSService payOSService,
            IDoctorService doctorService,
            IAppointmentStatusRepository appointmentStatusRepository)
        {
            _appointmentRepository = appointmentRepository;
            _zoomService = zoomService;
            _scheduleRepository = scheduleRepository;
            _doctorRepository = doctorRepository;
            _payOSService = payOSService;
            _doctorService = doctorService;
            _appointmentStatusRepository = appointmentStatusRepository;
        }

        public string RequestAppointmentAsync(RequestAppointmentDTO requestAppointment)
        {
            DoctorScheduleDTO schedule = CleanAndValidate(requestAppointment.DoctorScheduleID);

            // Create new pending appointment
            if (!requestAppointment.IsOnline)
            {
                requestAppointment.IsAnonymous = false;
            }

            var appointmentDTO = new AppointmentDTO
            {
                IsOnline = requestAppointment.IsOnline,
                DoctorScheduleID = requestAppointment.DoctorScheduleID,
                IsAnonymous = requestAppointment.IsAnonymous,
                PatientID = requestAppointment.PatientID,
                DoctorID = requestAppointment.DoctorID,
                Notes = requestAppointment.Notes,
                Reason = requestAppointment.Reason,
                StartTime = schedule.StartTime,
                ZoomLink = null,
                EndTime = schedule.EndTime,
                ScheduleDate = schedule.Date,
                AppointmentStatusID = AppointmentStatusEnum.Pending,
                CreatedDate = DateTime.Now
            };

            // Generate order code for new pending appointment
            int orderCode = GenerateUniqueOrderCode(appointmentDTO.AppointmentID);
            appointmentDTO.OrderCode = orderCode.ToString();

            // Insert pending appointment
            int appointmentId = _appointmentRepository.InsertAppointment(appointmentDTO);

            // Populate appointment information
            AppointmentDTO appointment2 = _appointmentRepository.GetAppointmentById(appointmentId);

            // Generate payment url
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
        }

        public AppointmentDTO GetAppointmentWithPatientDetailsById(int appointmentId)
        {
            var appointment = _appointmentRepository.GetAppointmentWithPatientDetailsById(appointmentId);

            if (appointment == null)
            {
                return null;
            }
            if (appointment.IsAnonymous)
            {
                appointment.PatientName = ConstraintWords.AnonymousName;
                appointment.PatientPhone = ConstraintWords.AnonymousName;
            }

            string latestDoctorNote = _appointmentRepository.GetLatestDoctorNotes(appointment.PatientID, appointment.DoctorID);
            appointment.DoctorNotes = latestDoctorNote;

            return appointment;
        }

        public async Task<AppointmentDTO> HandlePayOSWebhookAsync(string orderCode)
        {
            // Get the paid appointment
            var appointment = _appointmentRepository.GetAppointmentByOrderCode(orderCode);
            if (appointment == null)
                throw new Exception("Appointment not found.");

            // Return the appointment if the appointment is already confirmed
            if (appointment.AppointmentStatusID == AppointmentStatusEnum.Confirmed)
                return appointment;

            // Get total confirmed 
            int currentConfirmed = _appointmentRepository.GetQueueCountByScheduleId(
                appointment.DoctorScheduleID, new List<int>()
                {
                    AppointmentStatusEnum.Confirmed
                });
            // Check the worst situation (the payment is completed after the slot have been full)
            if (currentConfirmed >= Constraints.AmountOfPersonPerSchedule)
                throw new Exception("Appointment limit reached before payment was confirmed.");

            // Confirm the paid appointment
            _appointmentRepository.UpdateAppointmentStatus(appointment.AppointmentID, AppointmentStatusEnum.Confirmed);

            // Update queue number of the appointment
            int queueNumber = currentConfirmed + 1;
            _appointmentRepository.UpdateQueueNumber(appointment.AppointmentID, queueNumber);

            // Generate Zoom link
            if (appointment.IsOnline && string.IsNullOrEmpty(appointment.ZoomLink))
            {
                appointment.ZoomLink = await _zoomService.GenerateZoomLink(appointment);
                _appointmentRepository.UpdateZoomLink(appointment.AppointmentID, appointment.ZoomLink);
            } else
            {
                // Create and save QR code
                string domain = DeploymentSettingsContext.Settings.Domain;
                string url = $"{domain}/api/appointment/verify/{orderCode}";
                string fileName = $"{orderCode}.png";
                string saveFolder = Path.Combine(Directory.GetCurrentDirectory(), "uploads", "qrcodes");
                string publicQrUrl = $"{domain}/uploads/qrcodes/{orderCode}.png";
                QRCodeHelper.SaveAppointmentQrCode(url, saveFolder, fileName);
                EmailHelper.SendQrCodeEmail(
                    toEmail: appointment.PatientEmail,
                    doctorName: appointment.DoctorName,
                    appointmentDate: appointment.ScheduleDate,
                    startTime: appointment.StartTime,
                    qrUrl: publicQrUrl,
                    orderCode: orderCode
                );
            }

            // Return the full appointment information
            var doctorProfile = _doctorRepository.GetDoctorProfileByDoctorId(appointment.DoctorID);
            appointment.DoctorName = doctorProfile.Name;

            return appointment;
        }

        public AppointmentDTO GetAppointmentByOrderCode(int orderCode)
        {
            var appointment = _appointmentRepository.GetAppointmentByOrderCode(orderCode.ToString());
            if (appointment.AppointmentStatusID != AppointmentStatusEnum.Confirmed)
            {
                return null;
            }
            return appointment;
        }

        public void DoctorRequestReAppointment(RequestReAppointmentDTO request)
        {
            // Validate schedule
            var schedule = _scheduleRepository.GetScheduleById(request.DoctorScheduleID, DateTime.Now);
            if (schedule == null)
                throw new Exception("Không tìm thấy lịch.");

            if (schedule.DoctorScheduleStatus == DoctorScheduleStatusEnum.Close)
                throw new Exception("Lịch đã đóng.");

            // Check current confirmed/pending appointments
            int currentConfirmed = _appointmentRepository.GetQueueCountByScheduleId(
                request.DoctorScheduleID,
                new List<int> { AppointmentStatusEnum.Confirmed, AppointmentStatusEnum.Pending }
            );

            if (currentConfirmed >= Constraints.AmountOfPersonPerSchedule)
                throw new Exception("Số người đã đầy, không thể đặt thêm.");

            // Create confirmed appointment (offline, not anonymous)
            var appointment = new AppointmentDTO
            {
                IsOnline = false,
                IsAnonymous = false,
                DoctorScheduleID = request.DoctorScheduleID,
                DoctorID = request.DoctorID,
                PatientID = request.PatientID,
                Reason = ConstraintWords.ReAppointmentReason,
                Notes = request.Notes,
                DoctorNotes = request.DoctorNotes,
                StartTime = schedule.StartTime,
                EndTime = schedule.EndTime,
                ScheduleDate = schedule.Date,
                CreatedDate = DateTime.Now,
                AppointmentStatusID = AppointmentStatusEnum.Confirmed,
                QueueNumber = currentConfirmed + 1,
                ZoomLink = null
            };

            var orderCode = GenerateUniqueOrderCode(appointment.AppointmentID);
            appointment.OrderCode = orderCode.ToString();

            // Insert appointment
            int appointmentId = _appointmentRepository.InsertAppointment(appointment);

            // If schedule is full, update its status to closed
            if (appointment.QueueNumber >= Constraints.AmountOfPersonPerSchedule)
            {
                _scheduleRepository.UpdateScheduleStatus(request.DoctorScheduleID, DoctorScheduleStatusEnum.Close);
            }

            // Create and save QR code
            var appointment2 = _appointmentRepository.GetAppointmentByOrderCode(orderCode.ToString());
            string domain = DeploymentSettingsContext.Settings.Domain;
            string url = $"{domain}/api/appointment/verify/{orderCode}";
            string fileName = $"{orderCode}.png";
            string saveFolder = Path.Combine(Directory.GetCurrentDirectory(), "uploads", "qrcodes");
            string publicQrUrl = $"{domain}/uploads/qrcodes/{orderCode}.png";
            QRCodeHelper.SaveAppointmentQrCode(url, saveFolder, fileName);
            EmailHelper.SendQrCodeEmail(
                toEmail: appointment2.PatientEmail,
                doctorName: appointment2.DoctorName,
                appointmentDate: appointment2.ScheduleDate,
                startTime: appointment2.StartTime,
                qrUrl: publicQrUrl,
                orderCode: appointment2.OrderCode
            );
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

        public void UpdateAppointmentDoctorNote(DoctorAppointmentNoteDTO note)
        {
            _appointmentRepository.UpdateDoctorNoteAppointment(note);
        }

        public List<AppointmentDTO> GetAllAppointmentHasDoctorNotes(int doctorId)
        {
            var list = _appointmentRepository.GetAllAppointmentHasDoctorNotes(doctorId);

            foreach (AppointmentDTO appointment in list)
            {
                if (appointment.IsAnonymous)
                {
                    appointment.PatientName = ConstraintWords.AnonymousName;
                }
            }

            return list;
        }

        public List<AppointmentDTO> GetAllAppointments()
        {
            var list = _appointmentRepository.GetAllAppointments();

            foreach (AppointmentDTO appointment in list)
            {
                if (appointment.IsAnonymous)
                {
                    appointment.PatientName = ConstraintWords.AnonymousName;
                }
            }

            return list;
        }

        public void UpdateAppointmentStatus(UpdateAppointmentStatusDTO appointmentStatusDTO)
        {
            if (appointmentStatusDTO.AppointmentStatusID == AppointmentStatusEnum.Confirmed
                || appointmentStatusDTO.AppointmentStatusID == AppointmentStatusEnum.Pending)
            {
                CleanAndValidate(appointmentStatusDTO.ScheduleID);
                if (appointmentStatusDTO.AppointmentStatusID == AppointmentStatusEnum.Confirmed)
                {
                    int currentConfirmed = _appointmentRepository.GetQueueCountByScheduleId(
                        appointmentStatusDTO.ScheduleID,
                        new List<int> { AppointmentStatusEnum.Confirmed }
                    );

                    _appointmentRepository.UpdateQueueNumber(appointmentStatusDTO.AppointmentID, currentConfirmed + 1);
                }
            }

            if (appointmentStatusDTO.AppointmentStatusID == AppointmentStatusEnum.Cancelled
                || appointmentStatusDTO.AppointmentStatusID == AppointmentStatusEnum.Completed)
            {
                CancelAppointmentById(appointmentStatusDTO.AppointmentID);
            }

            _appointmentRepository.UpdateAppointmentStatus(appointmentStatusDTO);
        }

        public List<AppointmentStatusDTO> GetAllAppointmentStatus()
        {
            return _appointmentStatusRepository.GetAppointmentStatus();
        }

        public void UpdateAppointmentSchedule(UpdateScheduleAppointmentDTO update)
        {
            CleanAndValidate(update.DoctorScheduleID);
            var newAppointment = _appointmentRepository.UpdateAppointmentSchedule(update);

            if (newAppointment.IsOnline)
            {
                _zoomService.GenerateZoomLink(newAppointment);
            }
            else
            {
                EmailHelper.SendChangedAppointmentEmail(
                    newAppointment.PatientEmail,
                    newAppointment.DoctorName,
                    newAppointment.ScheduleDate,
                    newAppointment.StartTime,
                    newAppointment.EndTime,
                    newAppointment.Location);
            }
        }

        private DoctorScheduleDTO CleanAndValidate(int scheduleID)
        {
            // Lazy clean-up
            _doctorService.CleanUpAllSchedulesAvailability();

            // Validate the schedule
            var schedule = _scheduleRepository.GetScheduleById(scheduleID, DateTime.Now);
            if (schedule == null)
            {
                throw new Exception("Không tìm thấy lịch");
            }
            if (schedule.DoctorScheduleStatus == DoctorScheduleStatusEnum.Close)
            {
                throw new Exception("Lịch đã đóng");
            }
            else if (schedule.DoctorScheduleStatus == DoctorScheduleStatusEnum.Pending)
            {
                throw new Exception("Số người đang đăng ký đã vượt mức, vui lòng quay lại sau!");
            }
            ;

            return schedule;
        }
    }
}