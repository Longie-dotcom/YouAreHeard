namespace YouAreHeard.Models
{
    public class AppointmentDTO
    {
        public int AppointmentID { get; set; }
        public int MedicalHistoryID { get; set; }
        public int DoctorScheduleID { get; set; }
        public int? AppointmentStatusID { get; set; }
        public string? ZoomLink { get; set; }
        public string? Notes { get; set; }
        public string? Reason { get; set; }
        public bool IsAnonymous { get; set; }
        public string? AppointmentStatusName { get; set; }

        public string? PatientName { get; set; }
        public string? DoctorName { get; set; }
        public DateTime? Date { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public string? Location { get; set; }
        public int PatientID { get; set; }
        public int DoctorID { get; set; }
        public MedicalHistoryDTO? MedicalHistory { get; set; }
    }
}
