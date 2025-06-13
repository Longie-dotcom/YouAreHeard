using System.ComponentModel.DataAnnotations;

namespace YouAreHeard.Models
{
    public class UserDTO
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Tên không được để trống")]
        [RegularExpression(@"^[\p{L}\s]*$", ErrorMessage = "Tên không được chứa số hoặc ký tự đặc biệt")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Ngày sinh không được để trống")]
        public DateTime Dob { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string Phone { get; set; }

        public int RoleId { get; set; }
        public bool IsActive { get; set; }
    }
}
