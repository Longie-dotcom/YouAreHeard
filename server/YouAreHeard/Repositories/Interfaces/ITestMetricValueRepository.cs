using YouAreHeard.Models;

namespace YouAreHeard.Repositories.Interfaces
{
    public interface ITestMetricValueRepository
    {
        void InsertTestMetricValue(TestMetricValueDTO testMetricValue);
    }
}