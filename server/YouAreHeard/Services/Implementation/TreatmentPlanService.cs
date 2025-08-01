using YouAreHeard.Models;
using YouAreHeard.Repositories.Interfaces;
using YouAreHeard.Services.Interfaces;

namespace YouAreHeard.Services.Implementation
{
    public class TreatmentPlanService : ITreatmentPlanService
    {
        private readonly IARVRegimenRepository _ARVRegimenRepository;
        private readonly IMedicationRepository _medicationRepository;
        private readonly IPillRemindTimesRepository _pillRemindTimesRepository;
        private readonly ITreatmentPlanRepository _treatmentPlanRepository;

        public TreatmentPlanService(
            IARVRegimenRepository ARVRegimenRepository,
            IMedicationRepository medicationRepository,
            IPillRemindTimesRepository pillRemindTimesRepository,
            ITreatmentPlanRepository treatmentPlanRepository)
        {
            _ARVRegimenRepository = ARVRegimenRepository;
            _medicationRepository = medicationRepository;
            _pillRemindTimesRepository = pillRemindTimesRepository;
            _treatmentPlanRepository = treatmentPlanRepository;
        }

        public List<ARVRegimenDTO> GetARVRegimens()
        {
            return _ARVRegimenRepository.GetAllARVRegimens();
        }

        public List<MedicationDTO> GetMedications()
        {
            return _medicationRepository.GetAllMedications();
        }

        public void CreateTreatmentPlan(RequestTreatmentPlanDTO requestTreatmentPlan)
        {
            requestTreatmentPlan.TreatmentPlan.Date = DateTime.Now;
            int treatmentPlanId = _treatmentPlanRepository.insertTreatmentPlan(requestTreatmentPlan.TreatmentPlan);
            if (treatmentPlanId >= 0)
            {
                foreach (var pillRemindTime in requestTreatmentPlan.PillRemindTimes)
                {
                    pillRemindTime.TreatmentPlanID = treatmentPlanId;
                    _pillRemindTimesRepository.insertPillRemindTimes(pillRemindTime);
                }
            }
            else
            {
                throw new Exception("Failed to create treatment plan.");
            }
        }

        public TreatmentPlanDetailsDTO? GetLatestTreatmentPlanByPatientID(int patientId)
        {
            return _treatmentPlanRepository.GetLatestTreatmentPlanByPatientID(patientId);
        }
    }
}