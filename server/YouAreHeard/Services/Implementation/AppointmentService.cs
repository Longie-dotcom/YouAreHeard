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

        public async Task<AppointmentDTO> RequestAppointmentAsync(AppointmentDTO appointment, MedicalHistoryDTO medicalHistory)
        {
            if (!string.IsNullOrEmpty(appointment.ZoomLink) && appointment.ZoomLink.Trim().ToLower() == "yes")
            {
                var doctorScheduleDTO = _scheduleRepository.GetScheduleById(appointment.DoctorScheduleID, true, DateTime.Now);
                appointment.ZoomLink = await _zoomService.GenerateZoomLink(medicalHistory, doctorScheduleDTO);
            }

            int currentQueue = _appointmentRepository.GetQueueCountByScheduleId(appointment.DoctorScheduleID, AppointmentStatusEnum.Confirmed);
            if (currentQueue >= Constraint.AmountOfPersonPerSchedule)
            {
                throw new Exception("Maximum number of appointments reached.");
            }

            int queueNumber = currentQueue + 1;

            int medicalHistoryID = _appointmentRepository.InsertMedicalHistory(medicalHistory);
            appointment.MedicalHistoryID = medicalHistoryID;
            appointment.AppointmentStatusID = AppointmentStatusEnum.Confirmed;
            appointment.QueueNumber = queueNumber;

            int appointmentId = _appointmentRepository.InsertAppointment(appointment);

            if (queueNumber >= Constraint.AmountOfPersonPerSchedule)
            {
                _scheduleRepository.UpdateScheduleAvailability(appointment.DoctorScheduleID, false);
            }

            var fullAppointment = _appointmentRepository.GetAppointmentById(appointmentId);

            var schedule = _scheduleRepository.GetScheduleById(fullAppointment.DoctorScheduleID, DateTime.Now);
            var doctorProfile = _doctorRepository.GetDoctorProfileByDoctorId(medicalHistory.DoctorID);

            fullAppointment.StartTime = schedule.StartTime;
            fullAppointment.EndTime = schedule.EndTime;
            fullAppointment.Date = schedule.Date;
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
