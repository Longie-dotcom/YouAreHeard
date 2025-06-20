using Microsoft.Win32;

namespace YouAreHeard
{
    public class PatientProfileDTO
    {
        public int UserID { get; set; }
        public int HivStatusID { get; set; }
        public int PregnancyStatusID { get; set; }



        public float Height { get; set; }
        public float Weight { get; set; }
        public string Gender { get; set; }


        public string? PregnancyStatusName { get; set; }
        
        public string? HIVStatusName{ get; set; }
    }
}