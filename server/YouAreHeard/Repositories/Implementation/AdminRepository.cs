using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using YouAreHeard.Repositories.Interfaces;

namespace YouAreHeard.Repositories.Implementation
{
    public class AdminRepository : IAdminRepository
    {
        public int GetTotalUserCount()
        {
            using var conn = DBContext.GetConnection();
            conn.Open();

            var query = "SELECT COUNT(*) FROM [User]";
            using var cmd = new SqlCommand(query, conn);
            return (int)cmd.ExecuteScalar();
        }

        public int GetTotalDoctorCount()
        {
            using var conn = DBContext.GetConnection();
            conn.Open();

            var query = "SELECT COUNT(*) FROM DoctorProfile";
            using var cmd = new SqlCommand(query, conn);
            return (int)cmd.ExecuteScalar();
        }

        public int GetTotalPatientCount()
        {
            using var conn = DBContext.GetConnection();
            conn.Open();

            var query = "SELECT COUNT(*) FROM PatientProfile";
            using var cmd = new SqlCommand(query, conn);
            return (int)cmd.ExecuteScalar();
        }

        public int GetTotalAppointmentCount()
        {
            using var conn = DBContext.GetConnection();
            conn.Open();

            var query = "SELECT COUNT(*) FROM Appointment";
            using var cmd = new SqlCommand(query, conn);
            return (int)cmd.ExecuteScalar();
        }

        public int GetActiveTreatmentPlanCount()
        {
            using var conn = DBContext.GetConnection();
            conn.Open();

            var query = "SELECT COUNT(*) FROM TreatmentPlan";
            using var cmd = new SqlCommand(query, conn);
            return (int)cmd.ExecuteScalar();
        }

        public int GetTotalLabResultCount()
        {
            using var conn = DBContext.GetConnection();
            conn.Open();

            var query = "SELECT COUNT(*) FROM LabResult";
            using var cmd = new SqlCommand(query, conn);
            return (int)cmd.ExecuteScalar();
        }

        public double GetAverageDoctorRating()
        {
            using var conn = DBContext.GetConnection();
            conn.Open();

            var query = "SELECT AVG(CAST(rateValue AS FLOAT)) FROM DoctorRating";
            using var cmd = new SqlCommand(query, conn);
            var result = cmd.ExecuteScalar();
            return result != DBNull.Value ? Convert.ToDouble(result) : 0.0;
        }

        public Dictionary<string, int> GetAppointmentStatusBreakdown()
        {
            var result = new Dictionary<string, int>();

            using var conn = DBContext.GetConnection();
            conn.Open();

            var query = @"
                SELECT s.appointmentStatusName, COUNT(*) AS total
                FROM Appointment a
                JOIN AppointmentStatus s ON a.appointmentStatusID = s.appointmentStatusID
                GROUP BY s.appointmentStatusName";

            using var cmd = new SqlCommand(query, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                string status = reader["appointmentStatusName"].ToString();
                int count = Convert.ToInt32(reader["total"]);
                result[status] = count;
            }

            return result;
        }

        public Dictionary<string, int> GetDoctorAppointmentLoad()
        {
            var result = new Dictionary<string, int>();

            using var conn = DBContext.GetConnection();
            conn.Open();

            var query = @"
                SELECT u.name, COUNT(*) AS total
                FROM Appointment a
                JOIN [User] u ON a.doctorID = u.userID
                GROUP BY u.name";

            using var cmd = new SqlCommand(query, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                string doctorName = reader["name"].ToString();
                int count = Convert.ToInt32(reader["total"]);
                result[doctorName] = count;
            }

            return result;
        }

        public Dictionary<string, int> GetTopUsedMedications(int top = 5)
        {
            var result = new Dictionary<string, int>();

            using var conn = DBContext.GetConnection();
            conn.Open();

            var query = $@"
                SELECT m.medicationName, COUNT(*) AS usageCount
                FROM PillRemindTimes p
                JOIN Medication m ON p.medicationID = m.medicationID
                GROUP BY m.medicationName
                ORDER BY usageCount DESC
                OFFSET 0 ROWS FETCH NEXT {top} ROWS ONLY";

            using var cmd = new SqlCommand(query, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                string name = reader["medicationName"].ToString();
                int count = Convert.ToInt32(reader["usageCount"]);
                result[name] = count;
            }

            return result;
        }

        public Dictionary<string, int> GetMostOrderedTestTypes()
        {
            var result = new Dictionary<string, int>();

            using var conn = DBContext.GetConnection();
            conn.Open();

            var query = @"
                SELECT t.testTypeName, COUNT(*) AS total
                FROM LabResult l
                JOIN TestType t ON l.testTypeID = t.testTypeID
                GROUP BY t.testTypeName";

            using var cmd = new SqlCommand(query, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                string testType = reader["testTypeName"].ToString();
                int count = Convert.ToInt32(reader["total"]);
                result[testType] = count;
            }

            return result;
        }

        public Dictionary<string, int> GetUserRoleDistribution()
        {
            var result = new Dictionary<string, int>();

            using var conn = DBContext.GetConnection();
            conn.Open();

            var query = @"
                SELECT r.roleName, COUNT(*) AS total
                FROM [User] u
                JOIN [Role] r ON u.roleID = r.roleID
                GROUP BY r.roleName";

            using var cmd = new SqlCommand(query, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                string role = reader["roleName"].ToString();
                int count = Convert.ToInt32(reader["total"]);
                result[role] = count;
            }

            return result;
        }

        public int GetVerifiedOtpCount()
        {
            using var conn = DBContext.GetConnection();
            conn.Open();

            var query = "SELECT COUNT(*) FROM OTPVerification WHERE isVerified = 1";
            using var cmd = new SqlCommand(query, conn);
            return (int)cmd.ExecuteScalar();
        }

        public int GetUnverifiedOtpCount()
        {
            using var conn = DBContext.GetConnection();
            conn.Open();

            var query = "SELECT COUNT(*) FROM OTPVerification WHERE isVerified = 0";
            using var cmd = new SqlCommand(query, conn);
            return (int)cmd.ExecuteScalar();
        }
    }
}
