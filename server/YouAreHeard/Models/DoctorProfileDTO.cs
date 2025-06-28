namespace YouAreHeard.Models
{
    public class DoctorProfileDTO
    {
        public int UserID { get; set; }
        public string Specialties { get; set; }
        public int YearsOfExperience { get; set; }
        public string Image { get; set; }
        public string Gender { get; set; }
        public string Description { get; set; }
        public string LanguagesSpoken { get; set; }

        // Additional fields (merge from tables User and DoctorSchedule)
        public string Name { get; set; }
        public string Phone { get; set; }
        public string AvailableDays { get; set; }

    }
}