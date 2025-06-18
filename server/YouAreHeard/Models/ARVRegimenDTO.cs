namespace YouAreHeard.Models
{
    public class ARVRegimenDTO
    {
        public int RegimenID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Duration { get; set; }
        public string RegimenSideEffects { get; set; }
        public string RegimenIndications { get; set; }
        public string RegimenContraindications { get; set; }

        public List<MedicationDTO> Medications { get; set; } = new();
    }
}
