using System.ComponentModel.DataAnnotations;

namespace RidePal.Web.Models
{
    public class CreateUserViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Confirm password doesn't match! Try again !")]
        public string ConfirmPassword { get; set; }
    }
}