using Microsoft.AspNetCore.Mvc;
using YouAreHeard.Models;
using YouAreHeard.Services.Interfaces;

namespace YouAreHeard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TreatmentPlanController : ControllerBase
    {
        private readonly ITreatmentPlanService _treatmentPlanService;

        public TreatmentPlanController(ITreatmentPlanService treatmentPlanService)
        {
            _treatmentPlanService = treatmentPlanService;
        }

        [HttpGet("ARVRegimen/all")]
        public IActionResult GetAllARVRegimen()
        {
            try
            {
                var regimens = _treatmentPlanService.GetARVRegimens();
                return Ok(regimens);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to retrieve regimens.", error = ex.Message });
            }
        }

        [HttpGet("patientGroup/all")]
        public IActionResult GetAllPatientGroup()
        {
            try
            {
                var patientGroups = _treatmentPlanService.GetPatientGroups();
                return Ok(patientGroups);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to retrieve patient groups.", error = ex.Message });
            }
        }

        [HttpGet("medication/all")]
        public IActionResult GetAllMedications()
        {
            try
            {
                var medications = _treatmentPlanService.GetMedications();
                return Ok(medications);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to retrieve medications.", error = ex.Message });
            }
        }

        [HttpPost("treatmentPlan/create")]
        public IActionResult CreateTreatmentPlan([FromBody] RequestTreatmentPlanDTO createTreatmentPlan)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _treatmentPlanService.CreateTreatmentPlan(createTreatmentPlan);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to create treatment plan.", error = ex.Message });
            }
        }

        [HttpGet("patient/{patientId}")]
        public IActionResult GetAllPatientTreatment(int patientId)
        {
            try
            {
                var treatmentPlans = _treatmentPlanService.GetLatestTreatmentPlanByPatientID(patientId);
                return Ok(treatmentPlans);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to retrieve patient treatment plan.", error = ex.Message });
            }
        }
    }
}
