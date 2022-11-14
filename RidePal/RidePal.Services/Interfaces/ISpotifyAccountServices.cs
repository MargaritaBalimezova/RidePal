using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RidePal.Services.Interfaces
{
    public interface ISpotifyAccountServices
    {
        Task<string> GetToken(string clientId, string clientSecret);
    }
}
