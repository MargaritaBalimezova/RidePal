using RidePal.Services.Helpers;
using System.ComponentModel.DataAnnotations;

namespace RidePal.Services.Models
{
    public class ResetPasswordModel
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string Token { get; set; }

        [Required, DataType(DataType.Password)]
        [MinLength(Constants.USER_PASSWORD_MIN_LENGTH, ErrorMessage = "Password length should be more than 8 characters!")]
        public string NewPassword { get; set; }

        [Required, DataType(DataType.Password)]
        [Compare("NewPassword")]
        public string ConfirmNewPassword { get; set; }

        public bool IsSuccess { get; set; }
    }
}