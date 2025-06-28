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
        public void UpdateZoomLink(int appointmentId, string zoomLink);
        public void CancelExpiredPendingAppointmentsBySchedule(int scheduleId, DateTime now, int expirationMinutes);
    }
}