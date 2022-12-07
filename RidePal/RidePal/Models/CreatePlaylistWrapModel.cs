using RidePal.Services.Models;

namespace RidePal.WEB.Models
{
    public class CreatePlaylistWrapModel
    {
        public CreatePlaylistViewModel createPlaylist { get; set; }

        public TripQuerryParameters tripParameters { get; set; }
    }
}
