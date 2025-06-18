using YouAreHeard.Models;

namespace YouAreHeard.Repositories.Interfaces
{
    public interface ITestTypeRepository
    {
        public List<TestTypeDTO> GetAllTestTypes();

        public TestTypeDTO GetTestTypeById(int id);
    }
}   