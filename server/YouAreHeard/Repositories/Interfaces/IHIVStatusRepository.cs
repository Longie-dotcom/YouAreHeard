using YouAreHeard.Models;

namespace YouAreHeard.Repositories.Interfaces
{
    public interface IHIVStatusRepository
    {
        public List<HIVStatusDTO> GetAllHIVStatuses();
    }
}
