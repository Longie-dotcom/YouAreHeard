using Microsoft.AspNetCore.Mvc;
using YouAreHeard;
using YouAreHeard.Models;
using YouAreHeard.Services.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class PatientProfileController : ControllerBase
{
    private IPatientProfileService _patientProfileService;

    public PatientProfileController(IPatientProfileService patientProfileService)
    {
        _patientProfileService = patientProfileService;
    }

    [HttpGet("all")]
    public IActionResult GetAllPatientProfile()
    {
        var pps = _patientProfileService.GetAllPatientProfile();

        return Ok(pps);
    }

    [HttpGet("profile/{profileId}")]
    public IActionResult GetPatientProfileById(int profileId)
    {
        var pp = _patientProfileService.GetPatientProfileById(profileId);
        return Ok(pp);
    }

    [HttpPost("insert")]
    public IActionResult InsertPatientProfile([FromBody] PatientProfileDTO pp)
    {
        if (!ModelState.IsValid)
        {
            {
                return BadRequest(ModelState);
            }
        }
        _patientProfileService.InsertPatientProfile(pp);
        return Ok();
    }

    [HttpPost("updateHIVStatus")]
    public IActionResult UpdateHIVStatus([FromBody] UpdatePatientHIVStatusDTO update)
    {
        if (!ModelState.IsValid)
        {
            {
                return BadRequest(ModelState);
            }
        }
        _patientProfileService.UpdatePatientHIVStatus(update);
        return Ok();
    }


    [HttpGet("conditions")]
    public IActionResult GetAllConditions()
    {
        var conditions = _patientProfileService.GetAllConditions();
        return Ok(conditions);
    }

    [HttpGet("hivstatuses")]
    public IActionResult GetAllHIVStatuses()
    {
        var hivStatuses = _patientProfileService.GetAllHIVStatuses();
        return Ok(hivStatuses);
    }

    [HttpGet("pregnancystatuses")]
    public IActionResult GetAllPregnancyStatuses()
    {
        var pregnancyStatuses = _patientProfileService.GetAllPregnancyStatuses();
        return Ok(pregnancyStatuses);
    }
}