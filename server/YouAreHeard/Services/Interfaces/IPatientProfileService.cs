namespace YouAreHeard.Services.Interfaces
{
    public interface IPatientProfileService
    {
        void InsertPatientProfile(PatientProfileDTO pp);

        List<PatientProfileDTO> GetAllPatientProfile();

        PatientProfileDTO GetPatientProfileById(int id);
    }
}