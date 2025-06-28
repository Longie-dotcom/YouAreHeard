using Microsoft.Data.SqlClient;
using YouAreHeard.Models;
using YouAreHeard.Repositories.Interfaces;

namespace YouAreHeard.Repositories.Implementation
{
    public class PregnancyStatusRepository : IPregnancyStatusRepository
    {
        public List<PregnancyStatusDTO> GetAllPregnancyStatuses()
        {
            using var conn = DBContext.GetConnection();
            conn.Open();

            string query = @"SELECT * FROM PregnancyStatus";

            using var cmd = new SqlCommand(query, conn);
            using var reader = cmd.ExecuteReader();

            var PregnancyStatusList = new List<PregnancyStatusDTO>();

            while (reader.Read())
            {
                PregnancyStatusDTO status = new PregnancyStatusDTO()
                {
                    PregnancyStatusID = reader.GetInt32(reader.GetOrdinal("pregnancyStatusID")),
                    PregnancyStatusName = reader["pregnancyStatusName"]?.ToString()
                };
                PregnancyStatusList.Add(status);
            }
            return PregnancyStatusList;
        }
    }
}
