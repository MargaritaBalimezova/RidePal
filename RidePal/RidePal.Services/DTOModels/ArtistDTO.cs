using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RidePal.Services.DTOModels
{
    public class ArtistDTO
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<AlbumDTO> Albums { get; set; }

        public ICollection<TrackDTO> Tracks { get; set; }
    }
}
