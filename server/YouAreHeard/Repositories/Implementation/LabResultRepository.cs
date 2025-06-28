using Microsoft.Data.SqlClient;
using YouAreHeard.Models;
using YouAreHeard.Repositories.Interfaces;

namespace YouAreHeard.Repositories.Implementation
{
    public class LabResultRepository : ILabResultRepository
    {
        public List<LabResultDTO> GetAllLabResults()
        {
            using var conn = DBContext.GetConnection();
            conn.Open();

            string query = @"
            SELECT
                lr.labResultID,
                lr.testStageID,
                lr.testTypeID,
                lr.patientID,
                lr.doctorID,
                lr.date,
                lr.notes
            FROM LabResult lr
            ";

            using var cmd = new SqlCommand(query, conn);

            using var reader = cmd.ExecuteReader();

            var lrs = new List<LabResultDTO>();

            while (reader.Read())
            {
                var lr = new LabResultDTO
                {
                    LabResultId = reader.GetInt32(reader.GetOrdinal("labResultID")),
                    TestStageId = reader.GetInt32(reader.GetOrdinal("testStageID")),
                    TestTypeId = reader.GetInt32(reader.GetOrdinal("testTypeId")),
                    PatientId = reader.GetInt32(reader.GetOrdinal("patientID")),
                    DoctorId = reader.GetInt32(reader.GetOrdinal("doctorID")),
                    Date = reader.GetDateTime(reader.GetOrdinal("date")),
                    Note = reader.GetString(reader.GetOrdinal("notes")),

                };

                lrs.Add(lr);

            }

            return lrs;
        }

        public List<LabResultDTO> GetLabResultByDoctorId(int id)
        {
            using var conn = DBContext.GetConnection();
            conn.Open();

            string query = @"
            SELECT
                lr.labResultID,
                lr.testStageID,
                lr.testTypeID,
                lr.patientID,
                lr.doctorID,
                lr.date,
                lr.notes
            FROM LabResult lr
            WHERE lr.doctorID = @doctorID
            ";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@doctorID", id);

            using var reader = cmd.ExecuteReader();
            var lrs = new List<LabResultDTO>();
            while (reader.Read())
            {
                var lr = new LabResultDTO
                {
                    LabResultId = reader.GetInt32(reader.GetOrdinal("labResultID")),
                    TestStageId = reader.GetInt32(reader.GetOrdinal("testStageID")),
                    TestTypeId = reader.GetInt32(reader.GetOrdinal("testTypeId")),
                    PatientId = reader.GetInt32(reader.GetOrdinal("patientID")),
                    DoctorId = reader.GetInt32(reader.GetOrdinal("doctorID")),
                    Date = reader.GetDateTime(reader.GetOrdinal("date")),
                    Note = reader.GetString(reader.GetOrdinal("notes")),

                };
                lrs.Add(lr);
            }

            return lrs;
        }

        public List<LabResultDTO> GetLabResultByPatientId(int patientID)
        {
            using var conn = DBContext.GetConnection();
            conn.Open();

            string query = @"
                SELECT
                    lr.labResultID,
                    lr.testStageID,
                    ts.testStageName,
                    lr.testTypeID,
                    tt.testTypeName,
                    lr.patientID,
                    lr.doctorID,
                    lr.date,
                    lr.notes,
                    lr.isCustomized,

                    tm.testMetricID,
                    tm.testMetricName,
                    tm.unitName,
                    tmv.value
                FROM LabResult lr
                JOIN TestStage ts ON lr.testStageID = ts.testStageID
                JOIN TestType tt ON lr.testTypeID = tt.testTypeID
                LEFT JOIN TestMetricValue tmv ON lr.labResultID = tmv.labResultID
                LEFT JOIN TestMetric tm ON tmv.testMetricID = tm.testMetricID
                WHERE lr.patientID = @patientID
                ORDER BY lr.date DESC, lr.labResultID, tm.testMetricID";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@patientID", patientID);

            using var reader = cmd.ExecuteReader();

            var results = new List<LabResultDTO>();
            LabResultDTO currentResult = null;
            int lastResultId = -1;

            while (reader.Read())
            {
                int labResultID = reader.GetInt32(reader.GetOrdinal("labResultID"));

                if (labResultID != lastResultId)
                {
                    currentResult = new LabResultDTO
                    {
                        LabResultId = labResultID,
                        TestStageId = reader.GetInt32(reader.GetOrdinal("testStageID")),
                        TestStageName = reader.GetString(reader.GetOrdinal("testStageName")),
                        TestTypeId = reader.GetInt32(reader.GetOrdinal("testTypeID")),
                        TestTypeName = reader.GetString(reader.GetOrdinal("testTypeName")),
                        PatientId = reader.GetInt32(reader.GetOrdinal("patientID")),
                        DoctorId = reader.GetInt32(reader.GetOrdinal("doctorID")),
                        Date = reader.GetDateTime(reader.GetOrdinal("date")),
                        Note = reader.IsDBNull(reader.GetOrdinal("notes")) ? null : reader.GetString(reader.GetOrdinal("notes")),
                        IsCustomized = reader.GetBoolean(reader.GetOrdinal("isCustomized")),
                        TestMetricValues = new List<TestMetricValueDTO>()
                    };

                    results.Add(currentResult);
                    lastResultId = labResultID;
                }

                if (!reader.IsDBNull(reader.GetOrdinal("testMetricID")))
                {
                    var metric = new TestMetricValueDTO
                    {
                        LabResultID = labResultID,
                        TestMetricID = reader.GetInt32(reader.GetOrdinal("testMetricID")),
                        TestMetricName = reader.GetString(reader.GetOrdinal("testMetricName")),
                        UnitName = reader.IsDBNull(reader.GetOrdinal("unitName")) ? null : reader.GetString(reader.GetOrdinal("unitName")),
                        Value = reader.IsDBNull(reader.GetOrdinal("value")) ? null : reader.GetString(reader.GetOrdinal("value"))
                    };

                    currentResult.TestMetricValues.Add(metric);
                }
            }

            return results;
        }

        public int InsertLabResult(LabResultDTO lr)
        {
            using var conn = DBContext.GetConnection();
            conn.Open();

            string query = @"
            INSERT INTO LabResult
            (testStageID,testTypeID,patientID,doctorID,notes,isCustomized)
            OUTPUT INSERTED.labResultID
            VALUES
            (@testStageID, @testTypeID, @patientID, @doctorID, @notes, @isCustomized)
            ";

            using var cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@testStageID", lr.TestStageId);
            cmd.Parameters.AddWithValue("@testTypeID", lr.TestTypeId);
            cmd.Parameters.AddWithValue("@patientID", lr.PatientId);
            cmd.Parameters.AddWithValue("@doctorID", lr.DoctorId);
            cmd.Parameters.AddWithValue("@notes", lr.Note);
            cmd.Parameters.AddWithValue("@isCustomized", lr.IsCustomized);

            return (int)cmd.ExecuteScalar();
        }
    }
}