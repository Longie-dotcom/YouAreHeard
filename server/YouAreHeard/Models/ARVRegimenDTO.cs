namespace YouAreHeard.Models
{
    public class ARVRegimenDTO
    {
        public int RegimenID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Duration { get; set; }
        public string SideEffects { get; set; }
        public string Dosage { get; set; }
        public string Frequency { get; set; }
        public string Indications { get; set; }
        public string Contraindications { get; set; }

        public List<MedicationDTO> Medications { get; set; } = new();
    }
}
