using YouAreHeard.Models;

namespace YouAreHeard.Services.Interfaces
{
    public interface IAppointmentService
    {
        string RequestAppointmentAsync(RequestAppointmentDTO appointment);
        List<AppointmentDTO> GetAppointmentsByPatientId(int patientId);
        List<AppointmentDTO> GetAppointmentsByDoctorId(int doctorId);
        void CancelAppointmentById(int appointmentId);
        AppointmentDTO GetAppointmentWithPatientDetailsById(int appointmentId);
        Task<AppointmentDTO> HandlePayOSWebhookAsync(string orderCode);
    }
}