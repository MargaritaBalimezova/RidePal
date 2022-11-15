using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RidePal.Services.DTOModels
{
    public class LoginUserDTO
    {
        public int Id { get; set; }

        public int RoleId { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string ImagePath { get; set; }

        public bool IsEmailConfirmed { get; set; }

        public bool IsGoogleAccount { get; set; }
    }
}
