using YouAreHeard.Models;

namespace YouAreHeard.Services.Interfaces
{
    public interface IPatientProfileService
    {
        void InsertPatientProfile(PatientProfileDTO pp);

        List<PatientProfileDTO> GetAllPatientProfile();

        PatientProfileDTO GetPatientProfileById(int id);

        List<ConditionDTO> GetAllConditions();
        List<HIVStatusDTO> GetAllHIVStatuses();
        List<PregnancyStatusDTO> GetAllPregnancyStatuses();
    }
}