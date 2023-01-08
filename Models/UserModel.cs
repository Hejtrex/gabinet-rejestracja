using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace gabinet_rejestracja.Models
{
    public class UserModel
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} musi mieć przynajmniej {2} znaki i maksymalnie {1} znaków", MinimumLength = 2)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Podano dwa różne hasła!")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
