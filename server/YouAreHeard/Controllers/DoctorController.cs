using Microsoft.AspNetCore.Mvc;
using YouAreHeard.Models;
using YouAreHeard.Services.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

    [HttpGet("schedule/all/{roleID}")]
    public IActionResult GetAllDoctorSchedule(int roleID)
    {
        var schedules = _doctorService.GetAvailableDoctorSchedules(roleID);
        return Ok(schedules);
    }

    [HttpGet("all/{roleID}")]
    public IActionResult GetAllDoctors(int roleID)
    {
        var doctors = _doctorService.GetAllDoctorProfiles(roleID);
        return Ok(doctors);
    }

    [HttpPost("rating")]
    public IActionResult RateDoctor([FromBody] DoctorRatingDTO rating)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        _doctorService.RatingDoctor(rating);
        return Ok(new { message = "Đánh giá bác sĩ thành công." });
    }

    [HttpPost("profile/add")]
    public IActionResult AddDoctor([FromBody] DoctorProfileModel profile)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        _doctorService.RegisterDoctor(profile.UserDTO, profile.DoctorProfileDTO);
        return Ok(new { message = "Thêm bác sĩ thành công" });
    }

    [HttpPost("excelOfProfiles/add")]
    public async Task<IActionResult> AddDoctors([FromBody] List<DoctorProfileModel> profiles)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Track successful registrations
        var successfulDoctors = new List<(string Email, string Password)>();

        foreach (var profile in profiles)
        {
            bool result = await _doctorService.RegisterDoctor(profile.UserDTO, profile.DoctorProfileDTO);

            if (result)
            {
                successfulDoctors.Add((profile.UserDTO.Email, profile.UserDTO.Password));
            }
        }

        // Send emails to successful registrations
        if (successfulDoctors.Any())
        {
            await _doctorService.SendDoctorAccountEmailsAsync(successfulDoctors);
        }

        return Ok(new
        {
            total = profiles.Count,
            successful = successfulDoctors.Count,
            failed = profiles.Count - successfulDoctors.Count,
            message = successfulDoctors.Any()
                ? "Doctors registered successfully and emails sent"
                : "All doctor registrations failed"
        });
    }

    [HttpGet("doctorProfilesAdmin")]
    public IActionResult GetDoctorProfileByAdmin()
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var doctorList = _doctorService.GetAllDoctorProfilesByAdmin();
        return Ok(doctorList);
    }

    [HttpPost("excelOfSchedules/add")]
    public IActionResult AddSchedules([FromBody] List<DoctorScheduleDTO> schedules)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _doctorService.InsertSchedule(schedules);
        return Ok(new { message = "Thêm lịch làm việc thành công" });
    }
}