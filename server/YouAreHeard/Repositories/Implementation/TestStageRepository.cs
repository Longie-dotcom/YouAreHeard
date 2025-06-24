using YouAreHeard.Models;
using YouAreHeard.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using YouAreHeard.NewFolder;

namespace YouAreHeard.Repositories.Implementation
{
    public class TestStageRepository : ITestStageRepository
    {
        public List<TestStageDTO> GetAllTestStages()
        {
            using var conn = DBContext.GetConnection();
            conn.Open();

            string query = @"
                SELECT
                    ts.testStageID,
                    ts.testStageName
                FROM TestStage ts
            ";

            using var cmd = new SqlCommand(query, conn);
            using var reader = cmd.ExecuteReader();

            var tss = new List<TestStageDTO>();
            while (reader.Read())
            {
                var ts = new TestStageDTO
                {
                    testStageId = reader.GetInt32(reader.GetOrdinal("testStageID")),
                    testStageName = reader.GetString(reader.GetOrdinal("testStageName"))
                };
                tss.Add(ts);
            }

            return tss;
        }

        public TestStageDTO GetTestStageById(int id)
        {
            using var conn = DBContext.GetConnection();
            conn.Open();

            string query = @"
                SELECT
                    ts.testStageID,
                    ts.testStageName
                FROM TestStage ts
                WHERE ts.testStageID = @testStageID
            ";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@testStageID", id);
            using var reader = cmd.ExecuteReader();

            if (!reader.Read()) return null;

            return new TestStageDTO
            {
                testStageId = reader.GetInt32(reader.GetOrdinal("testStageID")),
                testStageName = reader.GetString(reader.GetOrdinal("testStageName"))
            };
            
        }
    }
}