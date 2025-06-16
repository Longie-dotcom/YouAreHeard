using YouAreHeard.Models;
using YouAreHeard.Repositories.Interfaces;
using YouAreHeard.Services.Interfaces;

namespace YouAreHeard.Services.Implementation
{
    public class TreatmentPlanService : ITreatmentPlanService
    {
        private readonly IARVRegimenRepository _ARVRegimenRepository;
        private readonly IPatientGroupRepository _patientGroupRepository;
        private readonly IMedicationRepository _medicationRepository;

        public TreatmentPlanService(
            IARVRegimenRepository ARVRegimenRepository, 
            IPatientGroupRepository patientGroupRepository,
            IMedicationRepository medicationRepository)
        {
            _ARVRegimenRepository = ARVRegimenRepository;
            _patientGroupRepository = patientGroupRepository;
            _medicationRepository = medicationRepository;
        }

        public List<ARVRegimenDTO> GetARVRegimens()
        {
            return _ARVRegimenRepository.GetAllARVRegimens();
        }

        public List<PatientGroupDTO> GetPatientGroups()
        {
            return _patientGroupRepository.GetAllPatientGroups();
        }

        public List<MedicationDTO> GetMedications()
        {
            return _medicationRepository.GetAllMedications();
        }
    }
}
