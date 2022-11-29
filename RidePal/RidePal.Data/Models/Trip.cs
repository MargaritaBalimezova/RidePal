using MovieForum.Data.Models.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace RidePal.Data.Models
{
    public class Trip : IHasId, IDeletable
    {
        public int Id { get; set; }

        [Required]
        public string StartPoint { get; set; }

        public string StartCoordinates { get; set; }

        [Required]
        public string Destination { get; set; }

        public string DestinationCoordinates { get; set; }

        [Required]
        public double Distance { get; set; }

        [Required]
        public double Duration { get; set; }

        [Required]
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}