namespace YouAreHeard.Models
{
    public class TestTypeDTO
    {
        public int TestTypeId { get; set; }
        public string TestTypeName { get; set; }
        public List<TestMetricDTO> TestMetrics { get; set; }
    }
}