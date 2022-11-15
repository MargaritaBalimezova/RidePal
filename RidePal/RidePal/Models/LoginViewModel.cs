using Microsoft.AspNetCore.Authentication;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RidePal.WEB.Models
{
    public class LoginViewModel
    {
        [Required]
        public string Credential { get; set; }

        [Required]
        public string Password { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; } 
        
    }
}
