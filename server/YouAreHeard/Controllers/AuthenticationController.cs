using Microsoft.AspNetCore.Mvc;
using YouAreHeard.Enums;
using YouAreHeard.Models;
using YouAreHeard.Services.Interfaces;

namespace YouAreHeard.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IOtpService _otpService;

        public AuthenticationController(IUserService userService, IOtpService otpService)
        {
            _userService = userService;
            _otpService = otpService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDTO newUser)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .SelectMany(kvp => kvp.Value.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(new { message = "Dữ liệu không hợp lệ", errors });
            }

            newUser.RoleId = RoleEnum.Patient;

            return await _userService.RegisterAsync(newUser);
        }

        [HttpPost("request-otp")]
        public IActionResult RequestOtp([FromBody] string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return BadRequest(new { message = "Email không được để trống." });
            }

            _otpService.GenerateAndSendOtp(email);

            return Ok(new { message = "OTP đã được gửi đến email của bạn." });
        }

        [HttpPost("verify-otp")]
        public IActionResult VerifyOtp([FromBody] VerifyOtpDTO dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .SelectMany(kvp => kvp.Value.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(new { message = "Dữ liệu không hợp lệ", errors });
            }

            var isVerified = _otpService.VerifyOtp(dto.Email, dto.OTP);
            if (!isVerified)
                return BadRequest(new { message = "OTP không hợp lệ hoặc đã hết hạn." });

            return Ok(new { message = "Xác thực OTP thành công." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .SelectMany(kvp => kvp.Value.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(new { message = "Dữ liệu không hợp lệ", errors });
            }

            return await _userService.LoginAsync(loginDTO);
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            return _userService.LogoutAsync().Result;
        }
    }
}