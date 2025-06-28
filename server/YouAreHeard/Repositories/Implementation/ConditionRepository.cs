using Microsoft.Data.SqlClient;
using YouAreHeard.Models;
using YouAreHeard.Repositories.Interfaces;

namespace YouAreHeard.Repositories.Implementation
{
    public class ConditionRepository : IConditionRepository
    {
        public List<ConditionDTO> GetAllConditions()
        {
            using var conn = DBContext.GetConnection();
            conn.Open();

            string query = @"SELECT * FROM Condition";

            using var cmd = new SqlCommand(query, conn);
            using var reader = cmd.ExecuteReader();

            var conditionList = new List<ConditionDTO>();

            while (reader.Read())
            {
                ConditionDTO condition = new ConditionDTO()
                {
                    ConditionID = reader.GetInt32(reader.GetOrdinal("conditionID")),
                    ConditionName = reader["conditionName"]?.ToString()
                };
                conditionList.Add(condition);
            }
            return conditionList;
        }
    }
}
