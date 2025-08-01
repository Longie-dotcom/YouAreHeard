using YouAreHeard.Models;

namespace YouAreHeard.Repositories.Interfaces
{
    public interface IPatientProfileRepository
    {
        public void InsertPatientProfile(PatientProfileDTO pp);

        public List<PatientProfileDTO> GetAllPatientProfile();

        public PatientProfileDTO GetPatientProfileById(int id);

        public bool IsPatientProfileExists(int userId);

        public void UpdatePatientProfile(PatientProfileDTO pp);
        public void UpdatePatientHIVStatus(UpdatePatientHIVStatusDTO update);
    }
}