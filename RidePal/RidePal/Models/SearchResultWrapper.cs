using RidePal.Services.DTOModels;
using System.Collections.Generic;

namespace RidePal.WEB.Models
{
    public class SearchResultWrapper
    {
        public IEnumerable<TrackDTO> Tracks { get; set; } = new List<TrackDTO>();
        public IEnumerable<UserDTO> Users { get; set; } = new List<UserDTO>();
        public IEnumerable<AlbumDTO> Albums { get; set; } = new List<AlbumDTO>();
        public IEnumerable<ArtistDTO> Artists { get; set; } = new List<ArtistDTO>();
        public IEnumerable<PlaylistDTO> Playlists { get; set; } = new List<PlaylistDTO>();
    }
}
