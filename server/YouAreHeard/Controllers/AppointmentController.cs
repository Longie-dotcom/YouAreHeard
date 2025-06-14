using Microsoft.AspNetCore.Mvc;
using YouAreHeard.Models;
using YouAreHeard.Services.Interfaces;

namespace YouAreHeard.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpPost("request")]
        public async Task<IActionResult> RequestAppointment([FromBody] AppointmentRequestDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _appointmentService.RequestAppointmentAsync(request.Appointment, request.MedicalHistory);

            return Ok(new { message = "Appointment requested successfully", zoomLink = request.Appointment.ZoomLink });
        }

        [HttpGet("patient/{patientId}")]
        public IActionResult GetAppointmentsByPatientId(int patientId)
        {
            try
            {
                var appointments = _appointmentService.GetAppointmentsByPatientId(patientId);
                return Ok(appointments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to retrieve appointments.", error = ex.Message });
            }
        }

        [HttpPut("cancel/{appointmentId}")]
        public IActionResult CancelAppointmentById(int appointmentId)
        {
            try
            {
                _appointmentService.CancelAppointmentById(appointmentId);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to retrieve appointments.", error = ex.Message });
            }
        }
    }
}
