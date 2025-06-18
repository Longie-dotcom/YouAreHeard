namespace YouAreHeard.Models
{
    public class AppointmentDTO
    {
        // Appointment data (table Appointment)
        public int AppointmentID { get; set; }
        public int DoctorScheduleID { get; set; }
        public int? AppointmentStatusID { get; set; }
        public string? ZoomLink { get; set; }
        public string? Notes { get; set; }
        public string? Reason { get; set; }
        public bool IsAnonymous { get; set; }
        public bool IsOnline { get; set; }
        public int? QueueNumber { get; set; }
        public int PatientID { get; set; }
        public int DoctorID { get; set; }
        public DateTime CreatedDate { get; set; }

        // Appointment status (table AppointmentStatus)
        public string? AppointmentStatusName { get; set; }

        // User(patient role) data (table User)
        public string? PatientName { get; set; }
        public string? PatientPhone { get; set; }
        public DateTime? PatientDob { get; set; }
        public PatientProfileDTO? PatientProfile { get; set; }

        // User(patient doctor) data (table User + DoctorProfile)
        public string? DoctorName { get; set; }
        public DoctorProfileDTO? DoctorProfile { get; set; }

        // Schedule data (table DoctorSchedule)
        public DateTime ScheduleDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Location { get; set; }
    }
}
