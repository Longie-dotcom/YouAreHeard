using Microsoft.Data.SqlClient;
using YouAreHeard.Models;
using YouAreHeard.Repositories.Interfaces;

namespace YouAreHeard.Repositories.Implementation
{
    public class DoctorRatingRepository : IDoctorRatingRepository
    {
        public void AddRating(DoctorRatingDTO rating)
        {
            using var conn = DBContext.GetConnection();
            conn.Open();

            string query = @"
                INSERT INTO DoctorRating (doctorID, userID, rateValue, description)
                VALUES (@doctorID, @userID, @rateValue, @description)
            ";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@doctorID", rating.DoctorID);
            cmd.Parameters.AddWithValue("@userID", rating.UserID);
            cmd.Parameters.AddWithValue("@rateValue", rating.RateValue);
            cmd.Parameters.AddWithValue("@description", rating.Description ?? (object)DBNull.Value);

            cmd.ExecuteNonQuery();
        }

        public void UpdateRating(DoctorRatingDTO rating)
        {
            using var conn = DBContext.GetConnection();
            conn.Open();

            string query = @"
                UPDATE DoctorRating
                SET rateValue = @rateValue, description = @description
                WHERE doctorID = @doctorID AND userID = @userID
            ";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@doctorID", rating.DoctorID);
            cmd.Parameters.AddWithValue("@userID", rating.UserID);
            cmd.Parameters.AddWithValue("@rateValue", rating.RateValue);
            cmd.Parameters.AddWithValue("@description", rating.Description ?? (object)DBNull.Value);

            cmd.ExecuteNonQuery();
        }

        public void DeleteRating(int doctorId, int userId)
        {
            using var conn = DBContext.GetConnection();
            conn.Open();

            string query = "DELETE FROM DoctorRating WHERE doctorID = @doctorID AND userID = @userID";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@doctorID", doctorId);
            cmd.Parameters.AddWithValue("@userID", userId);

            cmd.ExecuteNonQuery();
        }

        public DoctorRatingDTO? GetRating(int doctorId, int userId)
        {
            using var conn = DBContext.GetConnection();
            conn.Open();

            string query = "SELECT * FROM DoctorRating WHERE doctorID = @doctorID AND userID = @userID";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@doctorID", doctorId);
            cmd.Parameters.AddWithValue("@userID", userId);

            using var reader = cmd.ExecuteReader();
            if (!reader.Read()) return null;

            return new DoctorRatingDTO
            {
                DoctorID = reader.GetInt32(reader.GetOrdinal("doctorID")),
                UserID = reader.GetInt32(reader.GetOrdinal("userID")),
                RateValue = reader.GetInt32(reader.GetOrdinal("rateValue")),
                Description = reader.IsDBNull(reader.GetOrdinal("description")) ? null : reader.GetString(reader.GetOrdinal("description"))
            };
        }

        public List<DoctorRatingDTO> GetRatingsByDoctor(int doctorId)
        {
            var list = new List<DoctorRatingDTO>();
            using var conn = DBContext.GetConnection();
            conn.Open();

            string query = "SELECT * FROM DoctorRating WHERE doctorID = @doctorID";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@doctorID", doctorId);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new DoctorRatingDTO
                {
                    DoctorID = reader.GetInt32(reader.GetOrdinal("doctorID")),
                    UserID = reader.GetInt32(reader.GetOrdinal("userID")),
                    RateValue = reader.GetInt32(reader.GetOrdinal("rateValue")),
                    Description = reader.IsDBNull(reader.GetOrdinal("description")) ? null : reader.GetString(reader.GetOrdinal("description"))
                });
            }

            return list;
        }

        public double GetAverageRating(int doctorId)
        {
            using var conn = DBContext.GetConnection();
            conn.Open();

            string query = "SELECT AVG(CAST(rateValue AS FLOAT)) FROM DoctorRating WHERE doctorID = @doctorID";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@doctorID", doctorId);

            var result = cmd.ExecuteScalar();
            return result != DBNull.Value ? Convert.ToDouble(result) : 0.0;
        }
    }
}