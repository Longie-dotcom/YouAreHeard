using Microsoft.AspNetCore.Mvc;
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
    }
}
