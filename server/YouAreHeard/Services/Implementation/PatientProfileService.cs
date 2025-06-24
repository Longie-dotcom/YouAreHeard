using YouAreHeard.Models;
using YouAreHeard.Repositories.Interfaces;
using YouAreHeard.Services.Interfaces;

namespace YouAreHeard.Services
{
    public class PatientProfileService : IPatientProfileService
    {
        private readonly IPatientProfileRepository _patientProfileRepository;

        public PatientProfileService(IPatientProfileRepository patientProfileRepository)
        {
            _patientProfileRepository = patientProfileRepository;
        }

        public List<PatientProfileDTO> GetAllPatientProfile()
        {
            return _patientProfileRepository.GetAllPatientProfile();
        }

        public PatientProfileDTO GetPatientProfileById(int id)
        {
            return _patientProfileRepository.GetPatientProfileById(id);
        }

        public void InsertPatientProfile(PatientProfileDTO pp)
        {
            _patientProfileRepository.InsertPatientProfile(pp);
        }
    }
}