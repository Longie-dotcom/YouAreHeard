using YouAreHeard.Models;

namespace YouAreHeard.Repositories.Interfaces
{
    public interface IAppointmentRepository
    {
        public int InsertAppointment(AppointmentDTO appointment);
        public List<AppointmentDTO> GetAppointmentsByPatientId(int patientId, int statusId);
        public List<AppointmentDTO> GetAppointmentsByDoctorId(int doctorId, int statusId);
        public void UpdateAppointmentStatus(int appointmentId, int newStatusId);
        public int GetQueueCountByScheduleId(int scheduleId, int statusId);
        public int InsertMedicalHistory(MedicalHistoryDTO medicalHistory);
        public AppointmentDTO GetAppointmentById(int appointmentId);
        public List<AppointmentDTO> GetConfirmedAppointmentsByScheduleId(int scheduleId);
        public void UpdateQueueNumber(int appointmentId, int? newQueueNumber);
        public AppointmentDTO GetAppointmentWithPatientDetailsById(int appointmentId);
    }
}
