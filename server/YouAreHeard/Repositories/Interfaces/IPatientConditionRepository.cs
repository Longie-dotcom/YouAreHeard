using YouAreHeard.Models;

namespace YouAreHeard.Repositories.Interfaces
{
    public interface IPatientConditionRepository
    {
        public void InsertPatientCondition(ConditionDTO condition, int userID);
    }
}
