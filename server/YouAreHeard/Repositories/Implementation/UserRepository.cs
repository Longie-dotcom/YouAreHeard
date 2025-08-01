using Microsoft.Data.SqlClient;
using YouAreHeard.Models;
using YouAreHeard.Repositories.Interfaces;

namespace YouAreHeard.Repositories.Implementation
{
    public class UserRepository : IUserRepository
    {
        public bool EmailExists(string email)
        {
            using var conn = DBContext.GetConnection();
            conn.Open();

            string query = "SELECT COUNT(*) FROM [User] WHERE Email = @Email";
            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Email", email);
            int count = (int)cmd.ExecuteScalar();
            return count > 0;
        }

        public bool IsEmailVerified(string email)
        {
            using var conn = DBContext.GetConnection();
            conn.Open();

            string query = "SELECT IsVerified FROM OTPVerification WHERE Email = @Email";
            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Email", email);
            var result = cmd.ExecuteScalar();

            return result != null && result != DBNull.Value && Convert.ToBoolean(result);
        }

        public bool PhoneExists(string phone)
        {
            using var conn = DBContext.GetConnection();
            conn.Open();

            string query = "SELECT COUNT(*) FROM [User] WHERE Phone = @Phone";
            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Phone", phone);
            int count = (int)cmd.ExecuteScalar();
            return count > 0;
        }

        public bool InsertUser(UserDTO user)
        {
            using var conn = DBContext.GetConnection();
            conn.Open();

            string insertQuery = @"
                INSERT INTO [User] (Email, Password, Name, Dob, Phone, RoleId, IsActive)
                VALUES (@Email, @Password, @Name, @Dob, @Phone, @RoleId, @IsActive)";

            using var cmd = new SqlCommand(insertQuery, conn);
            cmd.Parameters.AddWithValue("@Email", user.Email);
            cmd.Parameters.AddWithValue("@Password", user.Password);
            cmd.Parameters.AddWithValue("@Name", user.Name);
            cmd.Parameters.AddWithValue("@Dob", user.Dob);
            cmd.Parameters.AddWithValue("@Phone", user.Phone);
            cmd.Parameters.AddWithValue("@RoleId", user.RoleId);
            cmd.Parameters.AddWithValue("@IsActive", true);

            return cmd.ExecuteNonQuery() > 0;
        }

        public UserDTO GetUserByEmail(string email)
        {
            using var conn = DBContext.GetConnection();
            conn.Open();

            string query = @"
                SELECT u.UserID, u.Email, u.Password, u.Name, u.Dob, u.Phone, u.RoleID, u.IsActive
                FROM [User] u
                WHERE u.Email = @Email";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Email", email);

            using var reader = cmd.ExecuteReader();
            if (!reader.Read()) return null;

            return new UserDTO
            {
                UserId = reader.GetInt32(reader.GetOrdinal("UserID")),
                Email = reader.GetString(reader.GetOrdinal("Email")),
                Password = reader.GetString(reader.GetOrdinal("Password")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                Dob = reader.GetDateTime(reader.GetOrdinal("Dob")),
                Phone = reader.GetString(reader.GetOrdinal("Phone")),
                RoleId = reader.GetInt32(reader.GetOrdinal("RoleID"))
            };
        }

        public UserDTO GetUserById(int id)
        {
            using var conn = DBContext.GetConnection();
            conn.Open();

            string query = @"
        SELECT u.UserID, u.Email, u.Password, u.Name, u.Dob, u.Phone, u.RoleID, u.IsActive
        FROM [User] u
        WHERE u.UserID = @UserID";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@UserID", id);

            using var reader = cmd.ExecuteReader();
            if (!reader.Read()) return null;

            return new UserDTO
            {
                UserId = reader.GetInt32(reader.GetOrdinal("UserID")),
                Email = reader.GetString(reader.GetOrdinal("Email")),
                Password = reader.GetString(reader.GetOrdinal("Password")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                Dob = reader.GetDateTime(reader.GetOrdinal("Dob")),
                Phone = reader.GetString(reader.GetOrdinal("Phone")),
                RoleId = reader.GetInt32(reader.GetOrdinal("RoleID")),
                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
            };
        }

        public string GetPSID(int userId)
        {
            throw new NotImplementedException();
        }

        public void SavePSID(int userId, string senderId)
        {
            using var conn = DBContext.GetConnection();
            conn.Open();

            string query = @"
                UPDATE [User]
                SET FacebookPSID = @PSID
                WHERE UserID = @UserID";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@PSID", senderId);
            cmd.Parameters.AddWithValue("@UserID", userId);

            cmd.ExecuteNonQuery();
        }
    }
}