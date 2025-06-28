using YouAreHeard.Models;

namespace YouAreHeard.Repositories.Interfaces
{
    public interface IConditionRepository
    {
        public List<ConditionDTO> GetAllConditions();
    }
}
