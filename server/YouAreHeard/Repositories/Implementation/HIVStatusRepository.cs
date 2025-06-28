using Microsoft.Data.SqlClient;
using YouAreHeard.Models;
using YouAreHeard.Repositories.Interfaces;

namespace YouAreHeard.Repositories.Implementation
{
    public class HIVStatusRepository : IHIVStatusRepository
    {
        public List<HIVStatusDTO> GetAllHIVStatuses()
        {
            using var conn = DBContext.GetConnection();
            conn.Open();

            string query = @"SELECT * FROM HIVStatus";

            using var cmd = new SqlCommand(query, conn);
            using var reader = cmd.ExecuteReader();

            var HIVStatusList = new List<HIVStatusDTO>();

            while (reader.Read())
            {
                HIVStatusDTO status = new HIVStatusDTO()
                {
                    HIVStatusID = reader.GetInt32(reader.GetOrdinal("HIVStatusID")),
                    HIVStatusName = reader["HIVStatusName"]?.ToString()
                };
                HIVStatusList.Add(status);
            }
            return HIVStatusList;
        }
    }
}
