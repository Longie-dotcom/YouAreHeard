using YouAreHeard.Models;
using Microsoft.Data.SqlClient;
using YouAreHeard.Repositories.Interfaces;

namespace YouAreHeard.Repositories.Implementation
{
    public class TestMetricRepository : ITestMetricRepository
    {
        public List<TestMetricDTO> GetAllTestMetrics()
        {
            using var conn = DBContext.GetConnection();
            conn.Open();

            string query = @"
                SELECT 
                    tm.testMetricID,
                    tm.testMetricName,
                    tm.unitName,
                    tm.testMetricTypeID,
                    tmt.testMetricTypeName
                FROM TestMetric tm
                JOIN TestMetricType tmt ON tm.testMetricTypeID = tmt.testMetricTypeID
            ";

            using var cmd = new SqlCommand(query, conn);
            using var reader = cmd.ExecuteReader();

            var metrics = new List<TestMetricDTO>();
            while (reader.Read())
            {
                var metric = new TestMetricDTO
                {
                    TestMetricID = reader.GetInt32(reader.GetOrdinal("testMetricID")),
                    TestMetricName = reader.GetString(reader.GetOrdinal("testMetricName")),
                    UnitName = reader.IsDBNull(reader.GetOrdinal("unitName"))
                        ? null : reader.GetString(reader.GetOrdinal("unitName")),
                    TestMetricTypeID = reader.GetInt32(reader.GetOrdinal("testMetricTypeID")),
                    TestMetricTypeName = reader.IsDBNull(reader.GetOrdinal("testMetricTypeName"))
                        ? null : reader.GetString(reader.GetOrdinal("testMetricTypeName"))
                };

                metrics.Add(metric);
            }

            return metrics;
        }
    }
}
