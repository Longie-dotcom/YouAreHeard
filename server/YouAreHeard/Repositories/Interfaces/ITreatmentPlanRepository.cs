using YouAreHeard.Models;

namespace YouAreHeard.Repositories.Interfaces
{
    public interface ITreatmentPlanRepository
    {
        public int insertTreatmentPlan(TreatmentPlanDTO treatmentPlan);
        public TreatmentPlanDetailsDTO? GetLatestTreatmentPlanByPatientID(int patientID);
    }
}