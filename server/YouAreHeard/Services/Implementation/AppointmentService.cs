using YouAreHeard.Models;
using YouAreHeard.Repositories.Interfaces;
using YouAreHeard.Services.Interfaces;

namespace YouAreHeard.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IZoomService _zoomService;

        public AppointmentService(IAppointmentRepository appointmentRepository, IZoomService zoomService, IScheduleRepository scheduleRepository)
        {
            _appointmentRepository = appointmentRepository;
            _zoomService = zoomService;
            _scheduleRepository = scheduleRepository;
        }

        public async Task RequestAppointmentAsync(AppointmentDTO appointment, MedicalHistoryDTO medicalHistory)
        {
            if (!string.IsNullOrEmpty(appointment.ZoomLink) && appointment.ZoomLink.Trim().ToLower() == "yes")
            {
                DoctorScheduleDTO doctorScheduleDTO = _scheduleRepository.GetDoctorScheduleById(appointment.DoctorScheduleID);
                appointment.ZoomLink = await _zoomService.GenerateZoomLink(medicalHistory, doctorScheduleDTO);
            }

            _appointmentRepository.RequestAppointment(appointment, medicalHistory);
        }

        public List<AppointmentDTO> GetAppointmentsByPatientId(int patientId)
        {
            return _appointmentRepository.GetAppointmentsByPatientId(patientId);
        }
    }
}
