using Microsoft.AspNetCore.Mvc;
using YouAreHeard.Models;
using YouAreHeard.Services;
using YouAreHeard.Services.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class LabResultController : ControllerBase
{
    private ILabResultService _LabResultService;

    public LabResultController(ILabResultService LabResultService)
    {
        _LabResultService = LabResultService;
    }

    [HttpGet("labResult/all")]
    public IActionResult GetAllLabResults()
    {
        var lrs = _LabResultService.GetAllLabResults();

        return Ok(lrs);
    }

    [HttpGet("testStage/all")]
    public IActionResult GetAllTestStages()
    {
        var tts = _LabResultService.GetAllTestStages();

        return Ok(tts);
    }

    [HttpGet("testType/all")]
    public IActionResult GetAllTestTypes()
    {
        var tts = _LabResultService.GetAllTestTypes();

        return Ok(tts);
    }

    [HttpGet("labResult/doctor/{doctorId}")]
    public IActionResult GetLabResultByDoctorId(int doctorId)
    {
        var lrs = _LabResultService.GetLabResultByDoctorId(doctorId);

        return Ok(lrs);
    }

    [HttpGet("labResult/patient/{patientId}")]
    public IActionResult GetLabResultByPatientId(int patientId)
    {
        var lrs = _LabResultService.GetLabResultByPatientId(patientId);

        return Ok(lrs);
    }
}