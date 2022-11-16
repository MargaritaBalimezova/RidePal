using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace RidePal.Web.Models
{
    public class UpdateUserViewModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Confirm password doesn't match! Type again !")]
        public string ConfirmPassword { get; set; }

        public IFormFile File { get; set; }
        public string ImagePath { get; set; }
    }
}