using YouAreHeard.Models;

namespace YouAreHeard.Repositories.Interfaces
{
    public interface IPatientGroupRepository
    {
        List<PatientGroupDTO> GetAllPatientGroups();
    }
}
