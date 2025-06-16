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
        var profile = _doctorService.GetDoctorProfileByDoctorId(int.Parse(userId));
        if (profile == null)
            return NotFound(new { message = "Không tìm thấy hồ sơ bác sĩ." });

        return Ok(profile);
    }

    [HttpGet("schedule/{userId}")]
    public IActionResult GetDoctorSchedule(string userId)
    {
        var schedule = _doctorService.GetAllAvailableDoctorScheduleByDoctorId(int.Parse(userId));
        return Ok(schedule);
    }

    [HttpGet("schedule/all")]
    public IActionResult GetAllDoctorSchedule()
    {
        var schedules = _doctorService.GetAvailableDoctorSchedules();
        return Ok(schedules);
    }

    [HttpGet("all")]
    public IActionResult GetAllDoctors()
    {
        var doctors = _doctorService.GetAllDoctorProfiles();
        return Ok(doctors);
    }
}
