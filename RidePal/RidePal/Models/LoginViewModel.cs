using System.ComponentModel.DataAnnotations;

namespace RidePal.WEB.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email/username and password combination doesn't match")]
        public string Credential { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "Email/username and password combination doesn't match")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

        public bool IsEmailConfirmed { get; set; }
        public bool IsBlocked { get; set; }
    }
}