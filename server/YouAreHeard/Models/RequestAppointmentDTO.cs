namespace YouAreHeard.Models
{
    public class RequestAppointmentDTO
    {
        public int DoctorScheduleID { get; set; }
        public string? Notes { get; set; }
        public string? Reason { get; set; }
        public bool IsAnonymous { get; set; }
        public bool IsOnline { get; set; }
        public int PatientID { get; set; }
        public int DoctorID { get; set; }
    }
}