using YouAreHeard.Models;

namespace YouAreHeard.Repositories.Interfaces
{
    public interface IARVRegimenRepository
    {
        List<ARVRegimenDTO> GetAllARVRegimens();
    }
}