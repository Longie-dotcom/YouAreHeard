using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;
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
                lr.doctorID
                lr.date,
                lr.note
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
                    testStageId = reader.GetInt32(reader.GetOrdinal("testStageID")),
                    testTypeId = reader.GetInt32(reader.GetOrdinal("testTypeId")),
                    patientId = reader.GetInt32(reader.GetOrdinal("patientID")),
                    doctorId = reader.GetInt32(reader.GetOrdinal("doctorID")),
                    date = reader.GetDateTime(reader.GetOrdinal("date")),
                    note = reader.GetString(reader.GetOrdinal("note")),

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
                lr.doctorID
                lr.date,
                lr.note
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
                    testStageId = reader.GetInt32(reader.GetOrdinal("testStageID")),
                    testTypeId = reader.GetInt32(reader.GetOrdinal("testTypeId")),
                    patientId = reader.GetInt32(reader.GetOrdinal("patientID")),
                    doctorId = reader.GetInt32(reader.GetOrdinal("doctorID")),
                    date = reader.GetDateTime(reader.GetOrdinal("date")),
                    note = reader.GetString(reader.GetOrdinal("note")),

                };
                lrs.Add(lr);
            }

            return lrs;
        }

        public List<LabResultDTO> GetLabResultByPatientId(int id)
        {
            using var conn = DBContext.GetConnection();
            conn.Open();

            string query = @"
            SELECT
                lr.labResultID,
                lr.testStageID,
                lr.testTypeID,
                lr.patientID,
                lr.doctorID
                lr.date,
                lr.note
            FROM LabResult lr
            WHERE lr.patientID = @patientID
            ";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@patientID", id);

            using var reader = cmd.ExecuteReader();
            var lrs = new List<LabResultDTO>();
            while (reader.Read())
            {
                var lr = new LabResultDTO
                {
                    LabResultId = reader.GetInt32(reader.GetOrdinal("labResultID")),
                    testStageId = reader.GetInt32(reader.GetOrdinal("testStageID")),
                    testTypeId = reader.GetInt32(reader.GetOrdinal("testTypeId")),
                    patientId = reader.GetInt32(reader.GetOrdinal("patientID")),
                    doctorId = reader.GetInt32(reader.GetOrdinal("doctorID")),
                    date = reader.GetDateTime(reader.GetOrdinal("date")),
                    note = reader.GetString(reader.GetOrdinal("note")),

                };
                lrs.Add(lr);
            }

            return lrs;
        }

        public int InsertLabResult(LabResultDTO lr)
        {
            using var conn = DBContext.GetConnection();
            conn.Open();

            string query = @"
            INSERT INTO LabResult
            {testStageID,testTypeID,patientID,doctorID}
            OUTPUT INSERTED.labResultID
            VALUES
            {@testStageID, @testTypeID, @patientID, @doctorID}
            ";

            using var cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@testStageID", lr.testStageId);
            cmd.Parameters.AddWithValue("@testTypeID", lr.testTypeId);
            cmd.Parameters.AddWithValue("@patientID", lr.patientId);
            cmd.Parameters.AddWithValue("@doctorID", lr.doctorId);

            return (int)cmd.ExecuteScalar();

        }
    }
}