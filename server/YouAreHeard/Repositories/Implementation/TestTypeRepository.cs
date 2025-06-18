using Microsoft.Data.SqlClient;
using YouAreHeard.Models;
using YouAreHeard.Repositories.Interfaces;

namespace YouAreHeard.Repositories.Implementation
{
    public class TestTypeRepository : ITestTypeRepository
    {
        public List<TestTypeDTO> GetAllTestTypes()
        {
            using var conn = DBContext.GetConnection();
            conn.Open();

            string query = @"
                SELECT
                    tt.testTypeID,
                    tt.testTypeName
                FROM TestType tt
            ";
            using var cmd = new SqlCommand(query, conn);
            using var reader = cmd.ExecuteReader();

            var tts = new List<TestTypeDTO>();
            while (reader.Read())
            {
                var tt = new TestTypeDTO
                {
                    testTypeId = reader.GetInt32(reader.GetOrdinal("testTypeID")),
                    testTypeName = reader.GetString(reader.GetOrdinal("testTypeName"))
                };
                tts.Add(tt);
            }

            return tts;
        }

        public TestTypeDTO GetTestTypeById(int id)
        {
            using var conn = DBContext.GetConnection();
            conn.Open();

            string query = @"
                SELECT
                    tt.testTypeID,
                    tt.testTypeName
                FROM TestType tt
                WHERE tt.testTypeID = @testTypeID
            ";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@testTypeID", id);
            using var reader = cmd.ExecuteReader();

            if (!reader.Read()) return null;

            return new TestTypeDTO
            {
                testTypeId = reader.GetInt32(reader.GetOrdinal("testTypeID")),
                testTypeName = reader.GetString(reader.GetOrdinal("testTypeName"))
            };
        }
    }
}