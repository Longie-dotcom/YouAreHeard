namespace YouAreHeard.Models
{
    public class ZoomSettings
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string AccountId { get; set; }
        public string DoctorDefaultEmail { get; set; } // optional fallback
    }
}
