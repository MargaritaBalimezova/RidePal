using RidePal.Data.Models;
using RidePal.Services.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RidePal.Services.Interfaces
{
    public interface ITrackServices 
    {
        Task<TrackDTO> GetByIdAsync(int id);
        IEnumerable<TrackDTO> GetTracksWithDistinctArtists(Genre genre, int duration);
        IEnumerable<TrackDTO> GetTracks(Genre genre, int duration);
        Task<IEnumerable<TrackDTO>> GetTracksByGenreAsync(Genre genre);
        Task<IEnumerable<TrackDTO>> GetAllTracksAsync();
        Task<IEnumerable<TrackDTO>> GetTopXTracksAsync(int x, Genre genre = null);
    }
}
