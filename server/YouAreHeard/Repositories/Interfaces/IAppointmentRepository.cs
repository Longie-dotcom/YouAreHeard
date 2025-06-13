using YouAreHeard.Models;

namespace YouAreHeard.Repositories.Interfaces
{
    public interface IAppointmentRepository
    {
        public void RequestAppointment(AppointmentDTO appointment, MedicalHistoryDTO medicalHistory);
        public List<AppointmentDTO> GetAppointmentsByPatientId(int patientId);
    }
}
