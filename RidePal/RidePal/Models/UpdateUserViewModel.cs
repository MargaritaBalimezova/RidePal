using Microsoft.AspNetCore.Http;
using RidePal.Services.Helpers;
using System.ComponentModel.DataAnnotations;

namespace RidePal.Web.Models
{
    public class UpdateUserViewModel
    {
        [MinLength(Constants.USER_FIRSTNAME_MIN_LENGTH, ErrorMessage = "First Name length should be more than 4 characters!")]
        public string FirstName { get; set; }

        [MinLength(Constants.USER_LASTNAME_MIN_LENGTH, ErrorMessage = "Last Name length should be more than 4 characters!")]
        public string LastName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [MinLength(Constants.USER_PASSWORD_MIN_LENGTH, ErrorMessage = "Password length should be more than 8 characters!")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Confirm password doesn't match! Type again !")]
        public string ConfirmPassword { get; set; }

        public IFormFile File { get; set; }
        public string ImagePath { get; set; }
    }
}