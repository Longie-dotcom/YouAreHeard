namespace YouAreHeard.Repositories.Interfaces
{
    public interface IPatientProfileRepository
    {
        public void InsertPatientProfile(PatientProfileDTO pp);

        public List<PatientProfileDTO> GetAllPatientProfile();

        public PatientProfileDTO GetPatientProfileById(int id);
    }
}