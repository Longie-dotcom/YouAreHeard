using Microsoft.AspNetCore.Mvc;
using YouAreHeard;
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
}