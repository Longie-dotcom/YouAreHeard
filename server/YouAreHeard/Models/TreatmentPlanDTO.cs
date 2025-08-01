namespace YouAreHeard.Models
{
    public class TreatmentPlanDTO
    {
        public int? TreatmentPlanID { get; set; }
        public int RegimenID { get; set; }
        public int PatientID { get; set; }
        public int DoctorID { get; set; }
        public DateTime? Date { get; set; }
        public string Notes { get; set; }
        public bool IsCustomized { get; set; }
    }
}