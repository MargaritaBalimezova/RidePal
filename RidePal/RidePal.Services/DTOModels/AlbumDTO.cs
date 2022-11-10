﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RidePal.Services.DTOModels
{
    public class AlbumDTO
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int? ArtistId { get; set; }

        public ArtistDTO Artist { get; set; }

        [Required]
        public int? GenreId { get; set; }

        public GenreDTO Genre { get; set; }

        public ICollection<TrackDTO> Tracks { get; set; }
    }
}