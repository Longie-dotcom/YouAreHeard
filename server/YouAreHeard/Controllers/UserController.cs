using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Net.Mail;
using System.Net;
using YouAreHeard.Models;

namespace YouAreHeard.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        [HttpPost("register")]
        public IActionResult Register([FromBody] UserDTO newUser)
        {
            // Validate model based on DataAnnotations
            if (!ModelState.IsValid)
            {
                var errorList = ModelState
                    .SelectMany(kvp => kvp.Value.Errors)
                    .Select(err => err.ErrorMessage)
                    .ToList();

                var response = new
                {
                    message = "Dữ liệu không hợp lệ",
                    errors = errorList
                };

                return new JsonResult(response) { StatusCode = 400 };
            }

            try
            {
                using (SqlConnection conn = DBContext.GetConnection())
                {
                    conn.Open();

                    bool emailExists = false;
                    bool phoneExists = false;

                    // Check email
                    string emailQuery = "SELECT COUNT(*) FROM [User] WHERE email = @email";
                    using (SqlCommand cemail = new SqlCommand(emailQuery, conn))
                    {
                        cemail.Parameters.AddWithValue("@email", newUser.Email);
                        emailExists = (int)cemail.ExecuteScalar() > 0;
                    }

                    if (emailExists)
                    {
                        // Check if the email is verified
                        string checkOtpQuery = "SELECT IsVerified FROM OtpVerification WHERE Email = @Email";
                        bool isVerified = false;

                        using (SqlCommand cmd = new SqlCommand(checkOtpQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@Email", newUser.Email);
                            object result = cmd.ExecuteScalar();
                            if (result != null && result != DBNull.Value)
                            {
                                isVerified = Convert.ToBoolean(result);
                            }
                        }

                        if (isVerified)
                        {
                            return BadRequest(new { message = "Email đã được đăng ký và xác minh. Không thể tiếp tục." });
                        }

                        // Re-send OTP
                        string otp = GenerateOTP();
                        SaveOtpToDatabase(newUser.Email, otp); // Update or Insert
                        SendOtpEmail(newUser.Email, otp);

                        return Ok(new { message = "OTP đã được gửi đến email của bạn. Vui lòng xác minh để hoàn tất đăng ký." });
                    }

                    // Check phone
                    string phoneQuery = "SELECT COUNT(*) FROM [User] WHERE Phone = @Phone";
                    using (SqlCommand cphone = new SqlCommand(phoneQuery, conn))
                    {
                        cphone.Parameters.AddWithValue("@Phone", newUser.Phone);
                        phoneExists = (int)cphone.ExecuteScalar() > 0;
                    }

                    if (phoneExists)
                    {
                        return BadRequest(new { message = "Số điện thoại đã được đăng ký" });
                    }

                    string insertQuery = @"INSERT INTO [User] 
                        (Email, Password, Name, Dob, Phone, RoleId, IsActive)
                        VALUES 
                        (@Email, @Password, @Name, @Dob, @Phone, @RoleId, @IsActive)";

                    using (SqlCommand insertCmd = new SqlCommand(insertQuery, conn))
                    {
                        insertCmd.Parameters.AddWithValue("@Email", newUser.Email);
                        insertCmd.Parameters.AddWithValue("@Password", newUser.Password);
                        insertCmd.Parameters.AddWithValue("@Name", newUser.Name);
                        insertCmd.Parameters.AddWithValue("@Dob", newUser.Dob);
                        insertCmd.Parameters.AddWithValue("@Phone", newUser.Phone);
                        insertCmd.Parameters.AddWithValue("@RoleId", newUser.RoleId);
                        insertCmd.Parameters.AddWithValue("@IsActive", true);

                        int rowsAffected = insertCmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            string otp = GenerateOTP();
                            SaveOtpToDatabase(newUser.Email, otp);
                            SendOtpEmail(newUser.Email, otp);

                            return Ok(new { message = "OTP đã được gửi đến email của bạn." });
                        }

                        return StatusCode(500, new { message = "Đã xảy ra lỗi khi đăng kí" });
                    }
                }
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "Đã xảy ra lỗi máy chủ. Vui lòng thử lại sau.", error = ex.Message });
            }
        }

        [HttpPost("request-otp")]
        public IActionResult RequestOtp([FromBody] string email)
        {
            if (!ModelState.IsValid)
            {
                var errorList = ModelState
                    .SelectMany(kvp => kvp.Value.Errors)
                    .Select(err => err.ErrorMessage)
                    .ToList();

                var response = new
                {
                    message = "Dữ liệu không hợp lệ",
                    errors = errorList
                };

                return new JsonResult(response) { StatusCode = 400 };
            }

            string otp = GenerateOTP();
            SaveOtpToDatabase(email, otp);
            SendOtpEmail(email, otp);

            return Ok(new { message = "OTP đã được gửi đến email của bạn." });
        }

        [HttpPost("verify-otp")]
        public IActionResult VerifyOtp([FromBody] VerifyOtpDTO dto)
        {
            if (!ModelState.IsValid)
            {
                var errorList = ModelState
                    .SelectMany(kvp => kvp.Value.Errors)
                    .Select(err => err.ErrorMessage)
                    .ToList();

                var response = new
                {
                    message = "Dữ liệu không hợp lệ",
                    errors = errorList
                };

                return new JsonResult(response) { StatusCode = 400 };
            }

            try
            {
                using (SqlConnection conn = DBContext.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT OTPID, expiredDate, isVerified 
                             FROM OTPVerification 
                             WHERE email = @Email AND OTP = @OTP";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", dto.Email);
                        cmd.Parameters.AddWithValue("@OTP", dto.OTP);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (!reader.Read())
                                return BadRequest(new { message = "OTP không hợp lệ." });

                            var expiredDate = reader.GetDateTime(reader.GetOrdinal("expiredDate"));
                            var isVerified = reader.GetBoolean(reader.GetOrdinal("isVerified"));
                            int otpId = reader.GetInt32(reader.GetOrdinal("OTPID"));

                            if (isVerified)
                                return BadRequest(new { message = "OTP đã được sử dụng." });

                            if (DateTime.Now > expiredDate)
                                return BadRequest(new { message = "OTP đã hết hạn." });

                            // Update OTP as verified
                            reader.Close();

                            string updateQuery = "UPDATE OTPVerification SET isVerified = 1 WHERE OTPID = @OTPID";
                            using (SqlCommand updateCmd = new SqlCommand(updateQuery, conn))
                            {
                                updateCmd.Parameters.AddWithValue("@OTPID", otpId);
                                updateCmd.ExecuteNonQuery();
                            }

                            return Ok(new { message = "Xác thực OTP thành công." });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi máy chủ. Vui lòng thử lại sau." });
            }
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
            {
                var errorList = ModelState
                    .SelectMany(kvp => kvp.Value.Errors)
                    .Select(err => err.ErrorMessage)
                    .ToList();

                return BadRequest(new { message = "Dữ liệu không hợp lệ", errors = errorList });
            }

            try
            {
                using (SqlConnection conn = DBContext.GetConnection())
                {
                    conn.Open();

                    // Check OTP verification 
                    string checkOTPQuery = @"
                    SELECT o.isVerified
                    FROM OTPVerification o
                    WHERE o.email = @Email";

                    using (SqlCommand otpCmd = new SqlCommand(checkOTPQuery, conn))
                    {
                        otpCmd.Parameters.AddWithValue("@Email", loginDTO.Email);

                        var isVerified = false;
                        using (var otpReader = otpCmd.ExecuteReader())
                        {
                            if (!otpReader.Read())
                            {
                                return Unauthorized(new { message = "Email chưa đăng ký OTP hoặc không tồn tại." });
                            }

                            isVerified = !otpReader.IsDBNull(0) && otpReader.GetBoolean(0);
                        }

                        if (!isVerified)
                        {
                            return Unauthorized(new { message = "Email chưa được xác minh. Vui lòng xác minh OTP." });
                        }
                    }

                    // Check password
                    string checkUserQuery = @"
                    SELECT u.UserID, u.Email, u.Password, u.Name, u.Dob, u.RoleID, u.Phone, u.IsActive, r.RoleName
                    FROM [User] u
                    LEFT JOIN [Role] r ON u.RoleID = r.RoleID
                    WHERE u.Email = @Email";

                    using (SqlCommand userCmd = new SqlCommand(checkUserQuery, conn))
                    {
                        userCmd.Parameters.AddWithValue("@Email", loginDTO.Email);

                        using (var reader = userCmd.ExecuteReader())
                        {
                            if (!reader.Read())
                                return Unauthorized(new { message = "Email không tồn tại trong hệ thống." });

                            var storedPassword = reader.GetString(reader.GetOrdinal("Password"));
                            if (storedPassword != loginDTO.Password)
                                return Unauthorized(new { message = "Mật khẩu không chính xác." });

                            // Lấy thông tin user
                            var userObject = new
                            {
                                UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Dob = reader.GetDateTime(reader.GetOrdinal("Dob")),
                                RoleID = reader.GetInt32(reader.GetOrdinal("RoleID")),
                                RoleName = reader.GetString(reader.GetOrdinal("RoleName")),
                                Phone = reader.GetString(reader.GetOrdinal("Phone")),
                                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
                            };

                            // Set cookie chứa user (JS-accessible)
                            string userJson = System.Text.Json.JsonSerializer.Serialize(userObject);

                            Response.Cookies.Append("user", userJson, new CookieOptions
                            {
                                HttpOnly = false,
                                Secure = true,
                                SameSite = SameSiteMode.None,
                                Expires = DateTimeOffset.UtcNow.AddDays(7)
                            });

                            return Ok(new { message = "Đăng nhập thành công." });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi máy chủ. Vui lòng thử lại sau.", error = ex.Message });
            }
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // Remove 'user' cookie by setting it to expire in the past
            Response.Cookies.Append("user", "", new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddDays(-1),
                HttpOnly = false, // match how it was set
                Secure = true,    // same as when it was created
                SameSite = SameSiteMode.None
            });

            return Ok(new { message = "Đăng xuất thành công." });
        }

        private void SendOtpEmail(string toEmail, string otp)
        {
            var settings = EmailSettingsContext.Settings;

            var fromAddress = new MailAddress(settings.From, settings.DisplayName);
            var toAddress = new MailAddress(toEmail);

            var smtp = new SmtpClient
            {
                Host = settings.SmtpHost,
                Port = settings.SmtpPort,
                EnableSsl = settings.EnableSSL,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(settings.Username, settings.Password)
            };

            using var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = "Your OTP Code",
                Body = $"Your OTP code is: {otp}. It expires in 5 minutes."
            };

            smtp.Send(message);
        }

        private string GenerateOTP(int length = 6)
        {
            var random = new Random();
            return string.Concat(Enumerable.Range(0, length).Select(_ => random.Next(0, 10).ToString()));
        }

        private void SaveOtpToDatabase(string email, string otp)
        {
            using (SqlConnection conn = DBContext.GetConnection())
            {
                conn.Open();

                // Check if an unverified OTP already exists
                string checkQuery = @"SELECT COUNT(*) FROM OTPVerification 
                              WHERE email = @Email AND isVerified = 0";

                using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@Email", email);
                    int count = (int)checkCmd.ExecuteScalar();

                    if (count > 0)
                    {
                        // Update existing unverified OTP
                        string updateQuery = @"UPDATE OTPVerification 
                                       SET OTP = @OTP, expiredDate = @ExpiredDate 
                                       WHERE email = @Email AND isVerified = 0";

                        using (SqlCommand updateCmd = new SqlCommand(updateQuery, conn))
                        {
                            updateCmd.Parameters.AddWithValue("@OTP", otp);
                            updateCmd.Parameters.AddWithValue("@ExpiredDate", DateTime.Now.AddMinutes(5));
                            updateCmd.Parameters.AddWithValue("@Email", email);
                            updateCmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        // Insert new OTP record
                        string insertQuery = @"INSERT INTO OTPVerification (OTP, email, expiredDate, isVerified)
                                       VALUES (@OTP, @Email, @ExpiredDate, 0)";
                        using (SqlCommand insertCmd = new SqlCommand(insertQuery, conn))
                        {
                            insertCmd.Parameters.AddWithValue("@OTP", otp);
                            insertCmd.Parameters.AddWithValue("@Email", email);
                            insertCmd.Parameters.AddWithValue("@ExpiredDate", DateTime.Now.AddMinutes(5));
                            insertCmd.ExecuteNonQuery();
                        }
                    }
                }
            }
        }
    }
}
