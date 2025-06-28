namespace YouAreHeard.Models
{
    public class TreatmentPlanDetailsDTO
    {
        // TreatmentPlan Info
        public int TreatmentPlanID { get; set; }
        public int RegimenID { get; set; }
        public int DoctorID { get; set; }
        public int PatientID { get; set; }
        public DateTime Date { get; set; }
        public string? Notes { get; set; }
        public bool IsCustomized { get; set; }

        // Regimen Info
        public string RegimenName { get; set; }
        public string RegimenType { get; set; }
        public string RegimenDuration { get; set; }
        public string RegimenSideEffects { get; set; }
        public string RegimenIndications { get; set; }
        public string RegimenContraindications { get; set; }

        // Pill Reminders List
        public List<PillRemindTimesDTO> PillRemindTimes { get; set; } = new();
    }
}