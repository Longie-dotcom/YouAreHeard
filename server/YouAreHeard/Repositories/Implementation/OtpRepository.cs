using Microsoft.Data.SqlClient;
using YouAreHeard.Repositories.Interfaces;

namespace YouAreHeard.Repositories.Implementation
{
    public class OtpRepository : IOtpRepository
    {
        public bool OtpExistsAndUnverified(string email)
        {
            using var conn = DBContext.GetConnection();
            conn.Open();

            string query = @"SELECT COUNT(*) FROM OTPVerification 
                             WHERE Email = @Email AND IsVerified = 0";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Email", email);
            int count = (int)cmd.ExecuteScalar();
            return count > 0;
        }

        public void InsertOrUpdateOtp(string email, string otp)
        {
            using var conn = DBContext.GetConnection();
            conn.Open();

            if (OtpExistsAndUnverified(email))
            {
                string updateQuery = @"
                    UPDATE OTPVerification 
                    SET OTP = @OTP, expiredDate = @ExpiredDate 
                    WHERE Email = @Email AND IsVerified = 0";

                using var cmd = new SqlCommand(updateQuery, conn);
                cmd.Parameters.AddWithValue("@OTP", otp);
                cmd.Parameters.AddWithValue("@ExpiredDate", DateTime.Now.AddMinutes(5));
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.ExecuteNonQuery();
            }
            else
            {
                string insertQuery = @"
                    INSERT INTO OTPVerification (OTP, Email, expiredDate, IsVerified)
                    VALUES (@OTP, @Email, @ExpiredDate, 0)";

                using var cmd = new SqlCommand(insertQuery, conn);
                cmd.Parameters.AddWithValue("@OTP", otp);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@ExpiredDate", DateTime.Now.AddMinutes(5));
                cmd.ExecuteNonQuery();
            }
        }

        public bool IsOtpValid(string email, string otp)
        {
            using var conn = DBContext.GetConnection();
            conn.Open();

            string query = @"SELECT expiredDate, IsVerified 
                             FROM OTPVerification 
                             WHERE Email = @Email AND OTP = @OTP";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@OTP", otp);

            using var reader = cmd.ExecuteReader();
            if (!reader.Read()) return false;

            var expiredDate = reader.GetDateTime(reader.GetOrdinal("expiredDate"));
            var isVerified = reader.GetBoolean(reader.GetOrdinal("IsVerified"));

            return !isVerified && DateTime.Now <= expiredDate;
        }

        public void MarkOtpAsVerified(string email, string otp)
        {
            using var conn = DBContext.GetConnection();
            conn.Open();

            string query = @"UPDATE OTPVerification 
                     SET IsVerified = 1 
                     WHERE Email = @Email AND OTP = @OTP";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@OTP", otp);
            cmd.ExecuteNonQuery();
        }
    }
}
