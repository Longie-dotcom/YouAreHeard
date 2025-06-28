namespace YouAreHeard.Models
{
    public class LabResultDTO
    {
        public int? LabResultId { get; set; }

        public int testStageId { get; set; }

        public int testTypeId { get; set; }

        public int patientId { get; set; }

        public int doctorId { get; set; }

        public DateTime date { get; set; }

        public string? note { get; set; }

        public bool IsCustomized { get; set; }

        public List<TestMetricValueDTO>? testMetricValues { get; set; }
    }
}