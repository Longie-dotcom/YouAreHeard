using Microsoft.Data.SqlClient;
using YouAreHeard.Models;
using YouAreHeard.Repositories.Interfaces;

namespace YouAreHeard.Repositories.Implementation
{
    public class TestMetricValueRepository : ITestMetricValueRepository
    {
        public void InsertTestMetricValue(TestMetricValueDTO testMetricValue)
        {
            using var conn = DBContext.GetConnection();
            conn.Open();

            string query = @"
                INSERT INTO TestMetricValue (labResultID, testMetricID, value)
                VALUES (@LabResultID, @TestMetricID, @Value);
            ";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@LabResultID", testMetricValue.LabResultID);
            cmd.Parameters.AddWithValue("@TestMetricID", testMetricValue.TestMetricID);
            cmd.Parameters.AddWithValue("@Value", testMetricValue.Value ?? (object)DBNull.Value);

            cmd.ExecuteNonQuery();
        }
    }
}