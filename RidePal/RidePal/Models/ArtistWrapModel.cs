using RidePal.Services.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RidePal.WEB.Models
{
    public class ArtistWrapModel
    {
        public IEnumerable<TrackDTO> TopTracks { get; set; } = new List<TrackDTO>();
        public IEnumerable<AlbumDTO> Albums { get; set; } = new List<AlbumDTO>();
        public ArtistDTO Artist { get; set; }
        public string Style { get; set; }
    }
}
