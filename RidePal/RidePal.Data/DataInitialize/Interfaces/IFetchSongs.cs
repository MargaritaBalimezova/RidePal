using RidePal.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RidePal.Data.DataInitialize.Interfaces
{
    public interface IFetchSongs
    {
        Task<IEnumerable<Track>> GetTracks(string genre);
    }
}
