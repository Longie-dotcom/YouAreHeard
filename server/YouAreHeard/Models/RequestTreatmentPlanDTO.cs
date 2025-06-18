namespace YouAreHeard.Models
{
    public class RequestTreatmentPlanDTO
    {
        public TreatmentPlanDTO TreatmentPlan { get; set; }
        public List<PillRemindTimesDTO> PillRemindTimes { get; set; }
    }
}
