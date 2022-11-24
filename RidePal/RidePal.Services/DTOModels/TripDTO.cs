using System.ComponentModel.DataAnnotations;

namespace RidePal.Services.DTOModels
{
    public class TripDTO
    {
        public int Id { get; set; }

        [Required]
        public string StartPoint { get; set; }

        [Required]
        public string Destination { get; set; }

        [Required]
        public double Distance { get; set; }

        [Required]
        public double Duration { get; set; }
    }
}