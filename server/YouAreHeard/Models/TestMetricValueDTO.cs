namespace YouAreHeard.Models
{
    public class TestMetricValueDTO
    {
        public int? LabResultID { get; set; }
        public int TestMetricID { get; set; }
        public string Value { get; set; }

        public string? TestMetricName { get; set; }
        public string? UnitName { get; set; }
    }
}
