using YouAreHeard.Models;

namespace YouAreHeard.Repositories.Interfaces
{
    public interface IMedicationRepository
    {
        public List<MedicationDTO> GetAllMedications();
    }
}
