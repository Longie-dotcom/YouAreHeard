namespace YouAreHeard.Models
{
    public class RequestReAppointmentDTO
    {
        public int DoctorScheduleID { get; set; }
        public string? Notes { get; set; }
        public string? DoctorNotes { get; set; }
        public int PatientID { get; set; }
        public int DoctorID { get; set; }
    }
}