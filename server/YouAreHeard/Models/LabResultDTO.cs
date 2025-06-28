namespace YouAreHeard.Models
{
    public class LabResultDTO
    {
        public int? LabResultId { get; set; }

        public int TestStageId { get; set; }
        public string? TestStageName { get; set; }

        public int TestTypeId { get; set; }
        public string? TestTypeName { get; set; }

        public int PatientId { get; set; }
        public int DoctorId { get; set; }

        public DateTime Date { get; set; }
        public string? Note { get; set; }

        public bool IsCustomized { get; set; }

        public List<TestMetricValueDTO>? TestMetricValues { get; set; }
    }
}