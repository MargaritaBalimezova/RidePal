using System.ComponentModel.DataAnnotations;

namespace RidePal.Services.Models
{
    public class TripQuerryParameters
    {
        [Required]
        public string StartPoint { get; set; }

        [Required]
        public string ArrivePoint { get; set; }

        [Required]
        public string StartingDestination { get; set; }

        [Required]
        public string ArrivingDestination { get; set; }


    }
}