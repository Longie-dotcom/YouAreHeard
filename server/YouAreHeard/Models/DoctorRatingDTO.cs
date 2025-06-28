namespace YouAreHeard.Models
{
    public class DoctorRatingDTO
    {
        public int DoctorID { get; set; }
        public int UserID { get; set; }
        public int RateValue { get; set; }
        public string? Description { get; set; }
    }
}