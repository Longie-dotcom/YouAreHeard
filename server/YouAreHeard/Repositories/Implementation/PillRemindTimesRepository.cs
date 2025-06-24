using Microsoft.Data.SqlClient;
using YouAreHeard.Repositories.Interfaces;
using YouAreHeard.Models;

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
    }
}
