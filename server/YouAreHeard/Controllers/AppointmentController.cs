﻿using Microsoft.AspNetCore.Mvc;
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
                return Redirect($"https://youareheard.life/successAppointment?orderCode={orderCode}");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error confirming appointment.", error = ex.Message });
            }
        }
    }
}