using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace gabinet_rejestracja.Models
{
    public class LoginModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
