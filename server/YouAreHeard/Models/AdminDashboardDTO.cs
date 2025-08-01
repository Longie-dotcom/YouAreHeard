namespace YouAreHeard.Models.DTOs
{
    public class AdminDashboardDTO
    {
        // Total counts
        public int TotalUsers { get; set; }
        public int TotalDoctors { get; set; }
        public int TotalPatients { get; set; }
        public int TotalAppointments { get; set; }
        public int ActiveTreatmentPlans { get; set; }
        public int TotalLabResults { get; set; }

        // Other stats
        public double AverageDoctorRating { get; set; }

        // Breakdowns
        public Dictionary<string, int> AppointmentStatusBreakdown { get; set; }
        public Dictionary<string, int> DoctorAppointmentLoad { get; set; }
        public Dictionary<string, int> TopUsedMedications { get; set; }
        public Dictionary<string, int> MostOrderedTestTypes { get; set; }
        public Dictionary<string, int> UserRoleDistribution { get; set; }

        // OTP
        public int VerifiedOtpCount { get; set; }
        public int UnverifiedOtpCount { get; set; }
    }
}
