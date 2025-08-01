using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using YouAreHeard.Models;
using YouAreHeard.Repositories.Interfaces;
using YouAreHeard.Services.Interfaces;

namespace YouAreHeard.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IOtpService _otpService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IUserRepository userRepository, IOtpService otpService, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _otpService = otpService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> RegisterAsync(UserDTO newUser)
        {
            if (_userRepository.EmailExists(newUser.Email))
            {
                if (_userRepository.IsEmailVerified(newUser.Email))
                {
                    return new BadRequestObjectResult(new { message = "Email đã được đăng ký và xác minh." });
                }

                _otpService.GenerateAndSendOtp(newUser.Email);
                return new OkObjectResult(new { message = "OTP đã được gửi. Vui lòng xác minh." });
            }

            if (_userRepository.PhoneExists(newUser.Phone))
            {
                return new BadRequestObjectResult(new { message = "Số điện thoại đã được đăng ký." });
            }

            if (_userRepository.InsertUser(newUser))
            {
                _otpService.GenerateAndSendOtp(newUser.Email);
                return new OkObjectResult(new { message = "Đăng ký thành công. OTP đã được gửi." });
            }

            return new StatusCodeResult(500);
        }

        public async Task<IActionResult> LoginAsync(LoginDTO loginDTO)
        {
            var user = _userRepository.GetUserByEmail(loginDTO.Email);

            if (user == null)
                return new UnauthorizedObjectResult(new { message = "Email không tồn tại." });

            if (!_userRepository.IsEmailVerified(loginDTO.Email))
                return new UnauthorizedObjectResult(new { message = "Email chưa được xác minh." });

            if (user.Password != loginDTO.Password)
                return new UnauthorizedObjectResult(new { message = "Mật khẩu không chính xác." });

            var userJson = JsonSerializer.Serialize(user);

            _httpContextAccessor.HttpContext.Response.Cookies.Append("user", userJson, new CookieOptions
            {
                HttpOnly = false,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            });

            return new OkObjectResult(new { message = "Đăng nhập thành công." });
        }

        public async Task<IActionResult> LogoutAsync()
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Append("user", "", new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddDays(-1),
                HttpOnly = false,
                Secure = true,
                SameSite = SameSiteMode.None
            });

            return new OkObjectResult(new { message = "Đăng xuất thành công." });
        }

        public UserDTO GetUserById(int id)
        {
            return _userRepository.GetUserById(id);
        }

        public void SaveFacebookPSID(int userId, string senderId)
        {
            _userRepository.SavePSID(userId, senderId);
        }
    }
}