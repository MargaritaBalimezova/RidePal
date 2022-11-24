using RidePal.Services.Helpers;
using System.ComponentModel.DataAnnotations;

namespace RidePal.Web.Models
{
    public class CreateUserViewModel
    {
        [Required]
        [MinLength(Constants.USER_USERNAME_MIN_LENGTH, ErrorMessage = "Username length should be more than 4 characters!")]
        public string Username { get; set; }

        [Required]
        [MinLength(Constants.USER_FIRSTNAME_MIN_LENGTH, ErrorMessage = "First Name length should be more than 4 characters!")]
        public string FirstName { get; set; }

        [Required]
        [MinLength(Constants.USER_LASTNAME_MIN_LENGTH, ErrorMessage = "Last Name length should be more than 4 characters!")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(Constants.USER_PASSWORD_MIN_LENGTH, ErrorMessage = "Password length should be more than 8 characters!")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Confirm password doesn't match! Try again !")]
        public string ConfirmPassword { get; set; }
    }
}