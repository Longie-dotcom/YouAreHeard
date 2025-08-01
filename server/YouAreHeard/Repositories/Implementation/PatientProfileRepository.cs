using Microsoft.Data.SqlClient;
using YouAreHeard.Models;
using YouAreHeard.Repositories.Interfaces;

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
                    Height = reader.GetDouble(reader.GetOrdinal("height")),
                    Weight = reader.GetDouble(reader.GetOrdinal("weight")),
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
                    u.name,
                    u.dob,
                    pp.hivStatusID,
                    hs.HIVStatusName,
                    pp.pregnancyStatusID,
                    ps.pregnancyStatusName,
                    pp.height,
                    pp.weight,
                    pp.gender,
                    pc.conditionID,
                    c.conditionName
                FROM PatientProfile pp
                LEFT JOIN [User] u ON pp.userID = u.userID
                LEFT JOIN HIVStatus hs ON pp.hivStatusID = hs.HIVStatusID
                LEFT JOIN PregnancyStatus ps ON pp.pregnancyStatusID = ps.pregnancyStatusID
                LEFT JOIN PatientCondition pc ON pp.userID = pc.userID
                LEFT JOIN [Condition] c ON pc.conditionID = c.conditionID
                WHERE pp.userID = @userID
            ";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@userID", id);
            using var reader = cmd.ExecuteReader();

            PatientProfileDTO patient = null;
            var conditions = new List<ConditionDTO>();

            while (reader.Read())
            {
                if (patient == null)
                {
                    patient = new PatientProfileDTO
                    {
                        UserID = reader.GetInt32(reader.GetOrdinal("userID")),
                        HivStatusID = reader.GetInt32(reader.GetOrdinal("hivStatusID")),
                        PregnancyStatusID = reader.GetInt32(reader.GetOrdinal("pregnancyStatusID")),
                        HIVStatusName = reader.IsDBNull(reader.GetOrdinal("HIVStatusName")) ? null : reader.GetString(reader.GetOrdinal("HIVStatusName")),
                        PregnancyStatusName = reader.IsDBNull(reader.GetOrdinal("pregnancyStatusName")) ? null : reader.GetString(reader.GetOrdinal("pregnancyStatusName")),
                        Height = reader.IsDBNull(reader.GetOrdinal("height")) ? null : reader.GetDouble(reader.GetOrdinal("height")),
                        Weight = reader.IsDBNull(reader.GetOrdinal("weight")) ? null : reader.GetDouble(reader.GetOrdinal("weight")),
                        Gender = reader.GetString(reader.GetOrdinal("gender")),
                        PatientName = reader.IsDBNull(reader.GetOrdinal("name")) ? null : reader.GetString(reader.GetOrdinal("name")),
                        PatientDob = reader.IsDBNull(reader.GetOrdinal("dob")) ? null : reader.GetDateTime(reader.GetOrdinal("dob")).ToString("yyyy-MM-dd"),
                        Conditions = conditions
                    };
                }

                if (!reader.IsDBNull(reader.GetOrdinal("conditionID")))
                {
                    conditions.Add(new ConditionDTO
                    {
                        ConditionID = reader.GetInt32(reader.GetOrdinal("conditionID")),
                        ConditionName = reader.IsDBNull(reader.GetOrdinal("conditionName"))
                                        ? null
                                        : reader.GetString(reader.GetOrdinal("conditionName"))
                    });
                }
            }

            return patient;
        }

        public void InsertPatientProfile(PatientProfileDTO pp)
        {
            using var conn = DBContext.GetConnection();
            conn.Open();

            string query = @"
            INSERT INTO PatientProfile (userID,hivStatusID,pregnancyStatusID,height,weight,gender)
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

        public bool IsPatientProfileExists(int userId)
        {
            using var conn = DBContext.GetConnection();
            conn.Open();

            string query = "SELECT COUNT(1) FROM PatientProfile WHERE userID = @userID";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@userID", userId);

            int count = (int)cmd.ExecuteScalar();
            return count > 0;
        }

        public void UpdatePatientProfile(PatientProfileDTO pp)
        {
            using var conn = DBContext.GetConnection();
            conn.Open();

            string query = @"
                UPDATE PatientProfile
                SET
                    pregnancyStatusID = @pregnancyStatusID,
                    height = @height,
                    weight = @weight,
                    gender = @gender
                WHERE userID = @userID
            ";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@userID", pp.UserID);
            cmd.Parameters.AddWithValue("@pregnancyStatusID", pp.PregnancyStatusID);
            cmd.Parameters.AddWithValue("@height", pp.Height);
            cmd.Parameters.AddWithValue("@weight", pp.Weight);
            cmd.Parameters.AddWithValue("@gender", pp.Gender);
            cmd.ExecuteNonQuery();
        }

        public void UpdatePatientHIVStatus(UpdatePatientHIVStatusDTO update)
        {
            using var conn = DBContext.GetConnection();
            conn.Open();

            string query = @"
                UPDATE PatientProfile
                SET
                    hivStatusID = @HivStatusID
                WHERE userID = @userID
            ";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@HivStatusID", update.HIVStatusID);
            cmd.Parameters.AddWithValue("@userID", update.PatientID);
            cmd.ExecuteNonQuery();
        }
    }
}
