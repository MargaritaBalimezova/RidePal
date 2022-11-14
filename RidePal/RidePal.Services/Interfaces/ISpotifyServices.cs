using RidePal.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RidePal.Services.Interfaces
{
    public interface ISpotifyServices
    {
        Task<IEnumerable<Track>> GetRapSongs(string token);
    }
}
