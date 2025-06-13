namespace YouAreHeard.Models
{
    public class AppointmentDTO
    {
        public int AppointmentID { get; set; }
        public int MedicalHistoryID { get; set; }
        public int DoctorScheduleID { get; set; }
        public int? AppointmentStatusID { get; set; }  // NEW
        public string? ZoomLink { get; set; }
        public string? Notes { get; set; }
        public string? Reason { get; set; }
        public bool IsAnonymous { get; set; }

        // Optional: for displaying status name
        public string? AppointmentStatusName { get; set; }
    }
}
