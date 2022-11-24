using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RidePal.Services.DTOModels
{
    public class ArtistDTO
    {
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<AlbumDTO> Albums { get; set; } = new List<AlbumDTO>();

        public ICollection<TrackDTO> Tracks { get; set; } = new List<TrackDTO>();
    }
}