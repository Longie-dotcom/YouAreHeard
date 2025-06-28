using YouAreHeard.Models;

namespace YouAreHeard.Repositories.Interfaces
{
    public interface IPregnancyStatusRepository
    {
        public List<PregnancyStatusDTO> GetAllPregnancyStatuses();
    }
}
