using System;

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

        public bool IsBlocked { get; set; }
        public int NumOfBlocks { get; set; }
        public DateTime LastBlockTime { get; set; }
    }
}