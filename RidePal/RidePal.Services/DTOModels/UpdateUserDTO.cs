using System.ComponentModel.DataAnnotations;

namespace RidePal.Services.DTOModels
{
    public class UpdateUserDTO
    {
        public int Id { get; set; }
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
        public string ImagePath { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
