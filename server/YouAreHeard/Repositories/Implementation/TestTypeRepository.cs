using Microsoft.Data.SqlClient;
using YouAreHeard.Models;
using YouAreHeard.Repositories.Interfaces;

namespace YouAreHeard.Repositories.Implementation
{
    public class TestTypeRepository : ITestTypeRepository
    {
        public List<TestTypeDTO> GetAllTestTypesWithMetrics()
        {
            using var conn = DBContext.GetConnection();
            conn.Open();

            string query = @"
                SELECT 
                    tt.testTypeID,
                    tt.testTypeName,
                    tm.testMetricID,
                    tm.testMetricName,
                    tm.unitName,
                    tm.testMetricTypeID,
                    tmt.testMetricTypeName
                FROM TestType tt
                LEFT JOIN TestMetricCombination tmc ON tt.testTypeID = tmc.testTypeID
                LEFT JOIN TestMetric tm ON tmc.testMetricID = tm.testMetricID
                LEFT JOIN TestMetricType tmt ON tm.testMetricTypeID = tmt.testMetricTypeID
                ORDER BY tt.testTypeID
            ";

            using var cmd = new SqlCommand(query, conn);
            using var reader = cmd.ExecuteReader();

            var testTypes = new List<TestTypeDTO>();
            TestTypeDTO? currentTestType = null;
            int? lastTestTypeId = null;

            while (reader.Read())
            {
                int testTypeId = reader.GetInt32(reader.GetOrdinal("testTypeID"));

                if (lastTestTypeId != testTypeId)
                {
                    currentTestType = new TestTypeDTO
                    {
                        TestTypeId = testTypeId,
                        TestTypeName = reader.GetString(reader.GetOrdinal("testTypeName")),
                        TestMetrics = new List<TestMetricDTO>()
                    };

                    testTypes.Add(currentTestType);
                    lastTestTypeId = testTypeId;
                }

                if (!reader.IsDBNull(reader.GetOrdinal("testMetricID")))
                {
                    var metric = new TestMetricDTO
                    {
                        TestMetricID = reader.GetInt32(reader.GetOrdinal("testMetricID")),
                        TestMetricName = reader.GetString(reader.GetOrdinal("testMetricName")),
                        UnitName = reader.GetString(reader.GetOrdinal("unitName")),
                        TestMetricTypeID = reader.GetInt32(reader.GetOrdinal("testMetricTypeID")),
                        TestMetricTypeName = reader.IsDBNull(reader.GetOrdinal("testMetricTypeName"))
                                             ? null
                                             : reader.GetString(reader.GetOrdinal("testMetricTypeName"))
                    };

                    currentTestType?.TestMetrics.Add(metric);
                }
            }

            return testTypes;
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
                TestTypeId = reader.GetInt32(reader.GetOrdinal("testTypeID")),
                TestTypeName = reader.GetString(reader.GetOrdinal("testTypeName"))
            };
        }
    }
}