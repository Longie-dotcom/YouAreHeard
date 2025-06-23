using YouAreHeard.Models;

namespace YouAreHeard.Repositories.Interfaces
{
    public interface ITestMetricRepository
    {
        public List<TestMetricDTO> GetAllTestMetrics();
    }
}
