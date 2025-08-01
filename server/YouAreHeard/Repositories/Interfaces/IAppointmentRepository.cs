using YouAreHeard.Models;

namespace YouAreHeard.Repositories.Interfaces
{
    public interface IAppointmentRepository
    {
        public int InsertAppointment(AppointmentDTO appointment);
        public List<AppointmentDTO> GetAppointmentsByPatientId(int patientId, int statusId);
        public List<AppointmentDTO> GetAppointmentsByDoctorId(int doctorId, int statusId);
        public void UpdateAppointmentStatus(int appointmentId, int newStatusId);
        public int GetQueueCountByScheduleId(int scheduleId, List<int> statusIds);
        public AppointmentDTO GetAppointmentById(int appointmentId);
        public AppointmentDTO GetAppointmentByOrderCode(string orderCode);
        public List<AppointmentDTO> GetConfirmedAppointmentsByScheduleId(int scheduleId);
        public void UpdateQueueNumber(int appointmentId, int? newQueueNumber);
        public AppointmentDTO GetAppointmentWithPatientDetailsById(int appointmentId);
        public string GetLatestDoctorNotes(int patientId, int doctorId);
        public void UpdateZoomLink(int appointmentId, string zoomLink);
        public void CancelExpiredPendingAppointmentsBySchedule(int scheduleId, DateTime now, int expirationMinutes);
        public void UpdateDoctorNoteAppointment(DoctorAppointmentNoteDTO note);
        public List<AppointmentDTO> GetAllAppointmentHasDoctorNotes(int doctorId);
        public List<AppointmentDTO> GetAllAppointments();
        public void UpdateAppointmentStatus(UpdateAppointmentStatusDTO appointmentStatusDTO);
        public AppointmentDTO UpdateAppointmentSchedule(UpdateScheduleAppointmentDTO update);
    }
}