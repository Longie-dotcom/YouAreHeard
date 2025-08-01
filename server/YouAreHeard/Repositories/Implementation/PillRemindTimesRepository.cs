using Microsoft.Data.SqlClient;
using YouAreHeard.Models;
using YouAreHeard.Repositories.Interfaces;

namespace YouAreHeard.Repositories.Implementation
{
    public class PillRemindTimesRepository : IPillRemindTimesRepository
    {
        public void insertPillRemindTimes(PillRemindTimesDTO pillRemindTimes)
        {
            using var conn = DBContext.GetConnection();
            conn.Open();
            string query = @"
                INSERT INTO PillRemindTimes 
                (TreatmentPlanID, Time, MedicationID, DrinkDosage, notes)
                VALUES
                (@TreatmentPlanID, @Time, @medicationID, @drinkDosage, @notes)";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@TreatmentPlanID", pillRemindTimes.TreatmentPlanID);
            cmd.Parameters.AddWithValue("@Time", TimeSpan.Parse(pillRemindTimes.Time));
            cmd.Parameters.AddWithValue("@medicationID", pillRemindTimes.MedicationID);
            cmd.Parameters.AddWithValue("@drinkDosage", pillRemindTimes.DrinkDosage);
            cmd.Parameters.AddWithValue("@notes", pillRemindTimes.Notes);
            cmd.ExecuteNonQuery();
        }

        public List<(string FacebookId, string MedicationName, string DosageMetric, int Dosage, TimeSpan Time)> GetPillRemindersDueNow()
        {
            var results = new List<(string, string, string, int, TimeSpan)>();
            var now = DateTime.Now.TimeOfDay;

            using var conn = DBContext.GetConnection();
            conn.Open();

            string query = @"
                WITH LatestPlans AS (
                    SELECT tp.*
                    FROM TreatmentPlan tp
                    INNER JOIN (
                        SELECT patientID, MAX([date]) AS LatestDate
                        FROM TreatmentPlan
                        GROUP BY patientID
                    ) latest ON tp.patientID = latest.patientID AND tp.[date] = latest.LatestDate
                )
                SELECT 
                    u.facebookPSID, 
                    m.medicationName, 
                    prt.time, 
                    prt.drinkDosage, 
                    m.dosageMetric
                FROM PillRemindTimes prt
                JOIN LatestPlans tp ON prt.treatmentPlanID = tp.treatmentPlanID
                JOIN [User] u ON tp.patientID = u.userID
                JOIN Medication m ON prt.medicationID = m.medicationID
                WHERE 
                    DATEDIFF(MINUTE, prt.time, @now) = 0
                    AND u.facebookPSID IS NOT NULL;";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@now", now);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var facebookId = reader["facebookPSID"].ToString();
                var medicationName = reader["medicationName"].ToString();
                var time = (TimeSpan)reader["time"];
                var dosage = (int)reader["drinkDosage"];
                var dosageMetric = reader["dosageMetric"].ToString();
                results.Add((facebookId, medicationName, dosageMetric, dosage, time));
            }
       
            return results;
        }
    }
}