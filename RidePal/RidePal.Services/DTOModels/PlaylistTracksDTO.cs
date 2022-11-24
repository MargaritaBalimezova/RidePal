using RidePal.Services.DTOModels;

namespace RidePal.Data.Models
{
    public class PlaylistTracksDTO
    {
        public long? TrackId { get; set; }
        public virtual TrackDTO Track { get; set; }
        public string Title { get; set; }
        public int? PlaylistId { get; set; }
    }
}