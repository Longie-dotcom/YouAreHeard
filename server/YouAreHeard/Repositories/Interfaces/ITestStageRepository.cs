using YouAreHeard.Models;
using YouAreHeard.Repositories.Interfaces;
using YouAreHeard.NewFolder;

namespace YouAreHeard.Repositories.Interfaces
{
    public interface ITestStageRepository
    {
        public List<TestStageDTO> GetAllTestStages();

        public TestStageDTO GetTestStageById(int id);
    }
}