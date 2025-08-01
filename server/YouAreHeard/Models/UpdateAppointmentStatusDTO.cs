namespace YouAreHeard.Models
{
    public class UpdateAppointmentStatusDTO
    {
        public int AppointmentID { get; set; }
        public int ScheduleID { get; set; }
        public int AppointmentStatusID { get; set; }
    }
}
