using RidePal.Data.DataInitialize.Models;
using RidePal.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RidePal.Data.DataInitialize.Interfaces
{
    public interface IFetchSongs
    {
        Task<PlaylistsResult> GetPlaylistAsync(string url);
        Task<ArtistTrackAlbumWrap> GetTracksAsync(string playlistsURL, Genre genre);
    }
}
