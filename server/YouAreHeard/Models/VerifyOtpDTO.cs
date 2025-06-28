using System.ComponentModel.DataAnnotations;

namespace YouAreHeard.Models
{
    public class VerifyOtpDTO
    {
        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        public string Email { get; set; }

        [Required(ErrorMessage = "OTP không được để trống")]
        public string OTP { get; set; }
    }
}