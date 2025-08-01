using YouAreHeard.Models;

namespace YouAreHeard.Repositories.Interfaces
{
    public interface IPatientConditionRepository
    {
        public void InsertPatientCondition(ConditionDTO condition, int userID);
        public void UpdatePatientConditions(int userID, List<ConditionDTO>? conditionIDs);
    }
}
