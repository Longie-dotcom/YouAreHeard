using YouAreHeard.Models;

namespace YouAreHeard.Services.Interfaces
{
    public interface IAppointmentService
    {
        Task<AppointmentDTO> RequestAppointmentAsync(RequestAppointmentDTO appointment);
        List<AppointmentDTO> GetAppointmentsByPatientId(int patientId);
        List<AppointmentDTO> GetAppointmentsByDoctorId(int doctorId);
        void CancelAppointmentById(int appointmentId);
        AppointmentDTO GetAppointmentWithPatientDetailsById(int appointmentId);
    }
}
