using Microsoft.AspNetCore.Mvc;
using YouAreHeard.Models;
using YouAreHeard.Services;
using YouAreHeard.Services.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class LabTestController : ControllerBase
{
    private ILabTestService _LabResultService;

    public LabTestController(ILabTestService LabResultService)
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
        var tts = _LabResultService.GetAllTestTypesWithMetrics();

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

    [HttpGet("metric/all")]
    public IActionResult GetAllMetrics()
    {
        var testMetrics = _LabResultService.GetTestMetrics();

        return Ok(testMetrics);
    }

    [HttpPost("create")]
    public IActionResult CreateLabResultTest([FromBody] LabResultDTO labResult)
    {
        _LabResultService.CreateLabResult(labResult);

        return Ok();
    }
}