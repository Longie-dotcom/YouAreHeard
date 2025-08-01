using Microsoft.AspNetCore.Mvc;
using YouAreHeard.Models;
using YouAreHeard.Services.Interfaces;
using YouAreHeard.Utilities;

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
        public IActionResult RequestAppointment([FromBody] RequestAppointmentDTO requestAppointment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                string appointmentId = _appointmentService.RequestAppointmentAsync(requestAppointment);
                return Ok(appointmentId);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
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

        [HttpGet("doctor/{doctorId}")]
        public IActionResult GetAppointmentsByDoctorId(int doctorId)
        {
            try
            {
                var appointments = _appointmentService.GetAppointmentsByDoctorId(doctorId);
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

        [HttpGet("getDetail/{appointmentId}")]
        public IActionResult GetPatientDetailByAppointmentId(int appointmentId)
        {
            try
            {
                var appointment = _appointmentService.GetAppointmentWithPatientDetailsById(appointmentId);
                return Ok(appointment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to retrieve appointments.", error = ex.Message });
            }
        }

        [HttpGet("return")]
        public async Task<IActionResult> PayOSReturn([FromQuery] int orderCode)
        {
            try
            {
                var result = await _appointmentService.HandlePayOSWebhookAsync(orderCode.ToString());
                return Redirect($"{DeploymentSettingsContext.Settings.Domain}/appointment-success/{result.OrderCode}");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error confirming appointment.", error = ex.Message });
            }
        }

        [HttpPost("reAppointment")]
        public IActionResult RequestReAppointment([FromBody] RequestReAppointmentDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _appointmentService.DoctorRequestReAppointment(request);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("doctorNote")]
        public IActionResult UpdateDoctorNote([FromBody] DoctorAppointmentNoteDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _appointmentService.UpdateAppointmentDoctorNote(request);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("appointmentsWithDoctorNote/{doctorId}")]
        public IActionResult GetAppointmentHasDoctorNotesOnly(int doctorId)
        {
            try
            {
                var appointment = _appointmentService.GetAllAppointmentHasDoctorNotes(doctorId);
                return Ok(appointment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to retrieve appointments with doctor notes.", error = ex.Message });
            }
        }

        [HttpGet("all")]
        public IActionResult GetAllAppointments()
        {
            try
            {
                var appointment = _appointmentService.GetAllAppointments();
                return Ok(appointment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to retrieve all appointments.", error = ex.Message });

            }
        }

        [HttpPost("updateStatus")]
        public IActionResult UpdateAppointmentStatus([FromBody] UpdateAppointmentStatusDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _appointmentService.UpdateAppointmentStatus(request);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("success/{orderCode}")]
        public IActionResult SuccessShow(int orderCode)
        {
            try
            {
                var appointment = _appointmentService.GetAppointmentByOrderCode(orderCode);
                return Ok(appointment);
            }
            catch (Exception e)
            {
                return StatusCode(500, new { message = e.Message });
            }
        }

        [HttpGet("appointmentStatus/all")]
        public IActionResult GetAllAppointmentStatus()
        {
            try
            {
                var statuses = _appointmentService.GetAllAppointmentStatus();
                return Ok(statuses);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to retrieve all appointment status.", error = ex.Message });
            }
        }

        [HttpPost("updateSchedule")]
        public IActionResult UpdateAppointmentSchedule([FromBody] UpdateScheduleAppointmentDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _appointmentService.UpdateAppointmentSchedule(request);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("verify/{orderCode}")]
        public IActionResult VerifyAppointment(int orderCode)
        {
            try
            {
                // Verify
                var appointment = _appointmentService.GetAppointmentByOrderCode(orderCode);

                if (appointment == null)
                {
                    return NotFound();
                }
                return Redirect($"{DeploymentSettingsContext.Settings.Domain}/verify-info/{orderCode}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("verify-identity/{orderCode}")]
        public IActionResult VerifyIdentityAppointment(int orderCode)
        {
            try
            {
                // Verify
                var appointment = _appointmentService.GetAppointmentByOrderCode(orderCode);

                if (appointment == null)
                {
                    return NotFound();
                }
                return Ok(appointment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}