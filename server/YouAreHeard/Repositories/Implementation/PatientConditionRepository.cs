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

        public void UpdatePatientConditions(int userID, List<ConditionDTO>? conditions)
        {
            using var conn = DBContext.GetConnection();
            conn.Open();
            
            if (conditions == null)
            {
                conditions = new List<ConditionDTO>();
            }

            using var transaction = conn.BeginTransaction();

            try
            {
                var deleteQuery = "DELETE FROM PatientCondition WHERE userID = @UserID";
                using (var deleteCmd = new SqlCommand(deleteQuery, conn, transaction))
                {
                    deleteCmd.Parameters.AddWithValue("@UserID", userID);
                    deleteCmd.ExecuteNonQuery();
                }

                var insertQuery = "INSERT INTO PatientCondition (userID, conditionID) VALUES (@UserID, @ConditionID)";
                foreach (var condition in conditions)
                {
                    using var insertCmd = new SqlCommand(insertQuery, conn, transaction);
                    insertCmd.Parameters.AddWithValue("@UserID", userID);
                    insertCmd.Parameters.AddWithValue("@ConditionID", condition.ConditionID);
                    insertCmd.ExecuteNonQuery();
                }

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}
