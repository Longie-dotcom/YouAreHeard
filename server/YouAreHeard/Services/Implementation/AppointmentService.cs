using YouAreHeard.Models;
using YouAreHeard.NewFolder;
using YouAreHeard.Repositories.Interfaces;
using YouAreHeard.Services.Interfaces;
using YouAreHeard.Enums;

namespace YouAreHeard.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IZoomService _zoomService;
        private readonly IDoctorRepository _doctorRepository;

        public AppointmentService(
            IAppointmentRepository appointmentRepository, 
            IZoomService zoomService, 
            IScheduleRepository scheduleRepository, 
            IDoctorRepository doctorRepository)
        {
            _appointmentRepository = appointmentRepository;
            _zoomService = zoomService;
            _scheduleRepository = scheduleRepository;
            _doctorRepository = doctorRepository;
        }

        public async Task<AppointmentDTO> RequestAppointmentAsync(RequestAppointmentDTO requestAppointment)
        {
            var schedule = _scheduleRepository.GetScheduleById(requestAppointment.DoctorScheduleID, true, DateTime.Now);
            if (schedule == null)
            {
                throw new Exception("Schedule not found.");
            }

            int currentQueue = _appointmentRepository.GetQueueCountByScheduleId(requestAppointment.DoctorScheduleID, AppointmentStatusEnum.Confirmed);
            if (currentQueue >= Constraint.AmountOfPersonPerSchedule)
            {
                throw new Exception("Maximum number of appointments reached.");
            }

            int queueNumber = currentQueue + 1;

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
                EndTime = schedule.EndTime,
                ScheduleDate = schedule.Date,
                AppointmentStatusID = AppointmentStatusEnum.Confirmed,
                CreatedDate = DateTime.Now
            };

            if (appointmentDTO.IsOnline)
            {
                appointmentDTO.ZoomLink = await _zoomService.GenerateZoomLink(appointmentDTO);
            }

            int appointmentId = _appointmentRepository.InsertAppointment(appointmentDTO);
            if (queueNumber >= Constraint.AmountOfPersonPerSchedule)
            {
                _scheduleRepository.UpdateScheduleAvailability(appointmentDTO.DoctorScheduleID, false);
            }

            var fullAppointment = _appointmentRepository.GetAppointmentById(appointmentId);
            var doctorProfile = _doctorRepository.GetDoctorProfileByDoctorId(appointmentDTO.DoctorID);

            fullAppointment.StartTime = schedule.StartTime;
            fullAppointment.EndTime = schedule.EndTime;
            fullAppointment.ScheduleDate = schedule.Date;
            fullAppointment.Location = schedule.Location;
            fullAppointment.DoctorName = doctorProfile.Name;

            return fullAppointment;
        }

        public List<AppointmentDTO> GetAppointmentsByPatientId(int patientId)
        {
            return _appointmentRepository.GetAppointmentsByPatientId(patientId, AppointmentStatusEnum.Confirmed);
        }

        public List<AppointmentDTO> GetAppointmentsByDoctorId(int doctorId)
        {
            return _appointmentRepository.GetAppointmentsByDoctorId(doctorId, AppointmentStatusEnum.Confirmed);
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
            int remainingConfirmed = _appointmentRepository.GetQueueCountByScheduleId(scheduleId, AppointmentStatusEnum.Confirmed);
            if (remainingConfirmed < Constraint.AmountOfPersonPerSchedule)
            {
                _scheduleRepository.UpdateScheduleAvailability(scheduleId, true);
            }
        }
        public AppointmentDTO GetAppointmentWithPatientDetailsById(int appointmentId)
        {
            return _appointmentRepository.GetAppointmentWithPatientDetailsById(appointmentId);
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
    }
}
