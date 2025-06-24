using YouAreHeard.Models;
using YouAreHeard.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using YouAreHeard.NewFolder;

namespace YouAreHeard.Repositories.Implementation
{
    public class PatientProfileRepository : IPatientProfileRepository
    {
        public List<PatientProfileDTO> GetAllPatientProfile()
        {
            using var conn = DBContext.GetConnection();
            conn.Open();

            string query = @"
                SELECT
                    pp.userID,
                    pp.hivStatusID,
                    pp.pregnancyStatusID,
                    pp.height,
                    pp.weight,
                    pp.gender
                FROM PatientProfile pp
            ";

            using var cmd = new SqlCommand(query, conn);
            using var reader = cmd.ExecuteReader();

            var pps = new List<PatientProfileDTO>();
            while (reader.Read())
            {
                var pp = new PatientProfileDTO
                {
                    UserID = reader.GetInt32(reader.GetOrdinal("userID")),
                    HivStatusID = reader.GetInt32(reader.GetOrdinal("hivStatusID")),
                    PregnancyStatusID = reader.GetInt32(reader.GetOrdinal("pregnancyStatusID")),
                    Height = reader.GetFloat(reader.GetOrdinal("height")),
                    Weight = reader.GetFloat(reader.GetOrdinal("weight")),
                    Gender = reader.GetString(reader.GetOrdinal("gender"))

                };
                pps.Add(pp);
            }

            return pps;
        }

        public PatientProfileDTO GetPatientProfileById(int id)
        {
            using var conn = DBContext.GetConnection();
            conn.Open();

            string query = @"
                SELECT
                    pp.userID,
                    pp.hivStatusID,
                    pp.pregnancyStatusID,
                    pp.height,
                    pp.weight,
                    pp.gender
                FROM PatientProfile pp
                WHERE pp.userID = @userID
            ";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@userID", id);
            using var reader = cmd.ExecuteReader();
            if (!reader.Read()) return null;

            return new PatientProfileDTO
            {
                UserID = reader.GetInt32(reader.GetOrdinal("userID")),
                HivStatusID = reader.GetInt32(reader.GetOrdinal("hivStatusID")),
                PregnancyStatusID = reader.GetInt32(reader.GetOrdinal("pregnancyStatusID")),
                Height = reader.GetFloat(reader.GetOrdinal("height")),
                Weight = reader.GetFloat(reader.GetOrdinal("weight")),
                Gender = reader.GetString(reader.GetOrdinal("gender"))
            };
        }

        public void InsertPatientProfile(PatientProfileDTO pp)
        {
            using var conn = DBContext.GetConnection();
            conn.Open();

            string query = @"
            INSERT INTO LabResult (userID,hivStatusID,pregnancyStatusID,height,weight,gender)
            VALUES (@userID,@hivStatusID,@pregnancyStatusID,@height,@weight,@gender)
            ";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@userID", pp.UserID);
            cmd.Parameters.AddWithValue("@hivStatusID", pp.HivStatusID);
            cmd.Parameters.AddWithValue("@pregnancyStatusID", pp.PregnancyStatusID);
            cmd.Parameters.AddWithValue("@height", pp.Height);
            cmd.Parameters.AddWithValue("@weight", pp.Weight);
            cmd.Parameters.AddWithValue("@gender", pp.Gender);
            cmd.ExecuteNonQuery();
        }
    }
}