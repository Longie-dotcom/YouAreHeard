using System.ComponentModel.DataAnnotations;

namespace YouAreHeard.Models
{
    public class UserDTO
    {
        private const string t1 = "Không được để trống!";
        private const string t2 = "Không đúng định dạng!";
        private const string t3 = "Phải có ít nhất 6 ký tự!";
        private const string t4 = "Không được chứa số hoặc ký tự đặc biệt!";
        public int UserId { get; set; }

        [Required(ErrorMessage = t1)]
        [EmailAddress(ErrorMessage = t2)]
        public string Email { get; set; }

        [Required(ErrorMessage = t1)]
        [MinLength(6, ErrorMessage = t3)]
        public string Password { get; set; }

        [Required(ErrorMessage = t1)]
        [RegularExpression(@"^[\p{L}\s]*$", ErrorMessage = t4)]
        public string Name { get; set; }

        [Required(ErrorMessage = t1)]
        public DateTime Dob { get; set; }

        [Required(ErrorMessage = t1)]
        [Phone(ErrorMessage = t2)]
        public string Phone { get; set; }

        public int RoleId { get; set; }
        public bool IsActive { get; set; }
    }
}
