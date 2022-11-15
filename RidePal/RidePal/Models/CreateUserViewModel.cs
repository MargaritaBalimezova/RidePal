using System.ComponentModel.DataAnnotations;

namespace RidePal.Web.Models
{
    public class CreateUserViewModel
    {
        [Required]
        [RegularExpression("^[A - Za - z0 - 9_ -] *$", ErrorMessage = "Please eneter a valid username!")]
        public string Username { get; set; }

        [Required]
        [RegularExpression("^[A - Za - z0 - 9_ -] *$", ErrorMessage = "Please eneter a valid first name!")]
        public string FirstName { get; set; }

        [Required]
        [RegularExpression("^[A - Za - z0 - 9_ -] *$" , ErrorMessage = "Please eneter a valid last name!")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression("[^ ]", ErrorMessage = "Password should be at least 8 chars long!")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Confirm password doesn't match! Try again !")]
        public string ConfirmPassword { get; set; }

    }
}
