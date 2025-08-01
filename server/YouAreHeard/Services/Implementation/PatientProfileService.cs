using YouAreHeard.Enums;
using YouAreHeard.Models;
using YouAreHeard.Repositories.Interfaces;
using YouAreHeard.Services.Interfaces;

namespace YouAreHeard.Services
{
    public class PatientProfileService : IPatientProfileService
    {
        private readonly IPatientProfileRepository _patientProfileRepository;
        private readonly IConditionRepository _conditionRepository;
        private readonly IHIVStatusRepository _hivStatusRepository;
        private readonly IPregnancyStatusRepository _pregnancyStatusRepository;
        private readonly IPatientConditionRepository _patientConditionRepository;

        public PatientProfileService(IPatientProfileRepository patientProfileRepository,
            IConditionRepository conditionRepository,
            IHIVStatusRepository HIVStatusRepository,
            IPregnancyStatusRepository pregnancyStatusRepository,
            IPatientConditionRepository patientConditionRepository)
        {
            _patientProfileRepository = patientProfileRepository;
            _conditionRepository = conditionRepository;
            _hivStatusRepository = HIVStatusRepository;
            _pregnancyStatusRepository = pregnancyStatusRepository;
            _patientConditionRepository = patientConditionRepository;
        }

        public List<ConditionDTO> GetAllConditions()
        {
            return _conditionRepository.GetAllConditions();
        }

        public List<HIVStatusDTO> GetAllHIVStatuses()
        {
            return _hivStatusRepository.GetAllHIVStatuses();
        }

        public List<PregnancyStatusDTO> GetAllPregnancyStatuses()
        {
            return _pregnancyStatusRepository.GetAllPregnancyStatuses();
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
            if (_patientProfileRepository.IsPatientProfileExists(pp.UserID))
            {
                _patientProfileRepository.UpdatePatientProfile(pp);
                _patientConditionRepository.UpdatePatientConditions(pp.UserID, pp.Conditions);
            }
            else
            {
                pp.HivStatusID = Constraints.DefaultHIVStatus;
                _patientProfileRepository.InsertPatientProfile(pp);
                foreach (var condition in pp.Conditions)
                {
                    _patientConditionRepository.InsertPatientCondition(condition, pp.UserID);
                }
            }
        }

        public void UpdatePatientHIVStatus(UpdatePatientHIVStatusDTO update)
        {
            _patientProfileRepository.UpdatePatientHIVStatus(update);
        }
    }
}