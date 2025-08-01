using YouAreHeard.Models;

namespace YouAreHeard
{
    public class PatientProfileDTO
    {
        public int UserID { get; set; }
        public int? HivStatusID { get; set; }
        public int PregnancyStatusID { get; set; }

        public double? Height { get; set; }
        public double? Weight { get; set; }
        public string? Gender { get; set; }

        public List<ConditionDTO> Conditions { get; set; }
        public string? PregnancyStatusName { get; set; }
        public string? HIVStatusName { get; set; }
        public string? PatientName { get; set; }
        public string? PatientDob { get; set; }
    }
}