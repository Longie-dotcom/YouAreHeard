namespace YouAreHeard.Repositories.Interfaces
{
    public interface IAdminRepository
    {
        // === TOTAL COUNTS ===
        int GetTotalUserCount();
        int GetTotalDoctorCount();
        int GetTotalPatientCount();
        int GetTotalAppointmentCount();
        int GetActiveTreatmentPlanCount();
        int GetTotalLabResultCount();
        double GetAverageDoctorRating();

        // === APPOINTMENT METRICS ===
        Dictionary<string, int> GetAppointmentStatusBreakdown();
        Dictionary<string, int> GetDoctorAppointmentLoad();

        // === TREATMENT / MEDICATION ===
        Dictionary<string, int> GetTopUsedMedications(int top = 5);

        // === LAB TESTS ===
        Dictionary<string, int> GetMostOrderedTestTypes();

        // === USERS ===
        Dictionary<string, int> GetUserRoleDistribution();
        int GetVerifiedOtpCount();
        int GetUnverifiedOtpCount();
    }
}
