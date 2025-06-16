using YouAreHeard.Models;

namespace YouAreHeard.Services.Interfaces
{
    public interface ITreatmentPlanService
    {
        List<ARVRegimenDTO> GetARVRegimens();
        List<PatientGroupDTO> GetPatientGroups();
        List<MedicationDTO> GetMedications();
    }
}
