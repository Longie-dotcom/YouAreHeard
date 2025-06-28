using YouAreHeard.Models;

namespace YouAreHeard.Repositories.Interfaces
{
    public interface ITestStageRepository
    {
        public List<TestStageDTO> GetAllTestStages();

        public TestStageDTO GetTestStageById(int id);
    }
}