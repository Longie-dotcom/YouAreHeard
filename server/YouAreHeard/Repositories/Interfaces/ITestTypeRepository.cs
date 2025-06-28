using YouAreHeard.Models;

namespace YouAreHeard.Repositories.Interfaces
{
    public interface ITestTypeRepository
    {
        public List<TestTypeDTO> GetAllTestTypesWithMetrics();

        public TestTypeDTO GetTestTypeById(int id);
    }
}