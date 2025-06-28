using Microsoft.Data.SqlClient;
using YouAreHeard.Models;
using YouAreHeard.Repositories.Interfaces;

namespace YouAreHeard.Repositories.Implementation
{
    public class PatientConditionRepository : IPatientConditionRepository
    {
        public void InsertPatientCondition(ConditionDTO condition, int userID)
        {
            using var conn = DBContext.GetConnection();
            conn.Open();
            string query = @"
                INSERT INTO PatientCondition 
                (userID, conditionID)
                VALUES
                (@UserID, @ConditionID)";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@UserID", userID);
            cmd.Parameters.AddWithValue("@ConditionID", condition.ConditionID);

            cmd.ExecuteNonQuery();
        }
    }
}
