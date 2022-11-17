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
        TrackDTO GetByIdAsync(int id);
        IEnumerable<Track> GetTracksWithDistinctArtists(Genre genre, int duration);
        IEnumerable<Track> GetTracks(Genre genre, int duration);
    }
}
