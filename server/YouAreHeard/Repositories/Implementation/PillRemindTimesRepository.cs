using Microsoft.Data.SqlClient;
using YouAreHeard.Repositories.Interfaces;
namespace YouAreHeard.Repositories.Implementation
{
    public class PillRemindTimesRepository : IPillRemindTimesRepository
    {
        public void insertPillRemindTimes(Models.PillRemindTimesDTO pillRemindTimes)
        {
            using var conn = DBContext.GetConnection();
            conn.Open();
            string query = @"
                INSERT INTO PillRemindTimes 
                (TreatmentPlanID, Time, MedicationID, DrinkDosage)
                VALUES
                (@TreatmentPlanID, @Time, @medicationID, @drinkDosage)";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@TreatmentPlanID", pillRemindTimes.TreatmentPlanID);
            cmd.Parameters.AddWithValue("@Time", pillRemindTimes.Time);
            cmd.Parameters.AddWithValue("@medicationID", pillRemindTimes.MedicationID);
            cmd.Parameters.AddWithValue("@drinkDosage", pillRemindTimes.DrinkDosage);
        }
    }
}
