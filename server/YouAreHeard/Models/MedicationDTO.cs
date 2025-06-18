namespace YouAreHeard.Models
{
    public class MedicationDTO
    {
        public int MedicationID { get; set; }
        public string MedicationName { get; set; }
        public string DosageMetric { get; set; }
        public string SideEffect { get; set; }
        public string Contraindications { get; set; }
        public string Indications { get; set; }

        // From MedicationCombination
        public int Dosage { get; set; }
        public int Frequency { get; set; }
    }
}
