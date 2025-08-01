namespace YouAreHeard.Models
{
    public class DoctorScheduleDTO
    {
        public int DoctorScheduleID { get; set; }
        public int UserID { get; set; }
        public string Location { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public DateTime Date { get; set; }
        public int DoctorScheduleStatus { get; set; }
        public string? DoctorScheduleStatusName { get; set; }
        public DoctorProfileDTO? DoctorProfile { get; set; }
    }
}