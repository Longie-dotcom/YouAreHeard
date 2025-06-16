using YouAreHeard.Models;

namespace YouAreHeard.Services.Interfaces
{
    public interface IAppointmentService
    {
        Task<AppointmentDTO> RequestAppointmentAsync(AppointmentDTO appointment, MedicalHistoryDTO medicalHistory);
        List<AppointmentDTO> GetAppointmentsByPatientId(int patientId);
        List<AppointmentDTO> GetAppointmentsByDoctorId(int doctorId);
        void CancelAppointmentById(int appointmentId);
        AppointmentDTO GetAppointmentWithPatientDetailsById(int appointmentId);
    }
}
