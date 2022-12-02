namespace RidePal.Services.DTOModels
{
    public class UpdateUserDTO
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string ImagePath { get; set; }

        public string Password { get; set; }
        public string AccessToken { get; set; }

        public bool IsGoogleAccount { get; set; }
        public bool IsEmailConfirmed { get; set; }
    }
}