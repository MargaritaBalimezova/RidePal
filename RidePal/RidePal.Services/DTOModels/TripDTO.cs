using RidePal.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

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

        public int PlaylistId { get; set; }

    }
}
