using Microsoft.AspNetCore.Mvc;
using YouAreHeard.Services.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class DoctorController : ControllerBase
{
    private readonly IDoctorService _doctorService;

    public DoctorController(IDoctorService doctorService)
    {
        _doctorService = doctorService;
    }

    [HttpGet("profile/{userId}")]
    public IActionResult GetDoctorProfile(string userId)
    {
        var profile = _doctorService.GetDoctorProfileByDoctorId(userId);
        if (profile == null)
            return NotFound(new { message = "Không tìm thấy hồ sơ bác sĩ." });

        return Ok(profile);
    }

    [HttpGet("schedule/{userId}")]
    public IActionResult GetDoctorSchedule(string userId)
    {
        var schedule = _doctorService.GetDoctorScheduleByDoctorId(userId);
        return Ok(schedule);
    }

    [HttpGet("all")]
    public IActionResult GetAllDoctors()
    {
        var doctors = _doctorService.GetAllDoctorProfiles();
        return Ok(doctors);
    }
}
