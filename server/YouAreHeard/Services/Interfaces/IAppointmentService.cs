using YouAreHeard.Models;

namespace YouAreHeard.Services.Interfaces
{
    public interface IAppointmentService
    {
        Task RequestAppointmentAsync(AppointmentDTO appointment, MedicalHistoryDTO medicalHistory);
        List<AppointmentDTO> GetAppointmentsByPatientId(int patientId);
    }
}
