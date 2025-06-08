using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using YouAreHeard.Models;

namespace YouAreHeard.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        [HttpPost("register")]
        public IActionResult Register([FromBody] UserDTO newUser)
        {
            // Validate model dựa vào DataAnnotations
            if (!ModelState.IsValid)
            {
                // Trả về lỗi validation cho client
                return BadRequest(ModelState);
            }

            try
            {
                using (SqlConnection conn = DBContext.GetConnection())
                {
                    conn.Open();

                    bool emailExists = false;
                    bool phoneExists = false;

                    // Check email
                    string emailQuery = "SELECT COUNT(*) FROM User WHERE UserEmail = @UserEmail";
                    using (SqlCommand cemail = new SqlCommand(emailQuery, conn))
                    {
                        cemail.Parameters.AddWithValue("@UserEmail", newUser.UserEmail);
                        emailExists = (int)cemail.ExecuteScalar() > 0;
                    }

                    // Check phone
                    string phoneQuery = "SELECT COUNT(*) FROM User WHERE PhoneNumber = @PhoneNumber";
                    using (SqlCommand cphone = new SqlCommand(phoneQuery, conn))
                    {
                        cphone.Parameters.AddWithValue("@PhoneNumber", newUser.PhoneNumber);
                        phoneExists = (int)cphone.ExecuteScalar() > 0;
                    }

                    if (emailExists || phoneExists)
                    {
                        var errors = new List<string>();
                        if (emailExists) errors.Add("Email đã được đăng kí.");
                        if (phoneExists) errors.Add("Số điện thoại đã được đăng kí.");
                        return BadRequest(new { messages = errors });
                    }

                    string insertQuery = @"INSERT INTO User 
                        (UserEmail, Password, Name, Dob, Gender, PhoneNumber, RoleId, IsActive)
                        VALUES 
                        (@UserEmail, @Password, @Name, @Dob, @Gender, @PhoneNumber, @RoleId, @IsActive)";

                    using (SqlCommand insertCmd = new SqlCommand(insertQuery, conn))
                    {
                        insertCmd.Parameters.AddWithValue("@UserEmail", newUser.UserEmail);
                        insertCmd.Parameters.AddWithValue("@Password", newUser.Password);
                        insertCmd.Parameters.AddWithValue("@Name", newUser.Name);
                        insertCmd.Parameters.AddWithValue("@Dob", newUser.Dob.ToDateTime(TimeOnly.MinValue));
                        insertCmd.Parameters.AddWithValue("@Gender", newUser.Gender);
                        insertCmd.Parameters.AddWithValue("@PhoneNumber", newUser.PhoneNumber);
                        insertCmd.Parameters.AddWithValue("@RoleId", newUser.RoleId);
                        insertCmd.Parameters.AddWithValue("@IsActive", true);

                        int rowsAffected = insertCmd.ExecuteNonQuery();

                        return rowsAffected > 0
                            ? Ok(new { message = "Tài khoản đã được đăng kí thành công." })
                            : StatusCode(500, new { message = "Đã xảy ra lỗi khi đăng kí" });
                    }
                }
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "Đã xảy ra lỗi máy chủ. Vui lòng thử lại sau.", error = ex.Message });
            }
        }
    }
}
