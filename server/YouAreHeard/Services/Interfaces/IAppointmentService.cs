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
        void DoctorRequestReAppointment(RequestReAppointmentDTO request);
        void UpdateAppointmentDoctorNote(DoctorAppointmentNoteDTO note);
        List<AppointmentDTO> GetAllAppointmentHasDoctorNotes(int doctorId);
        List<AppointmentDTO> GetAllAppointments();
        void UpdateAppointmentStatus(UpdateAppointmentStatusDTO appointmentStatusDTO);
        List<AppointmentStatusDTO> GetAllAppointmentStatus();
        void UpdateAppointmentSchedule(UpdateScheduleAppointmentDTO update);
        AppointmentDTO GetAppointmentByOrderCode(int orderCode);
    }
}