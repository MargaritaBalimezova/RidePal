using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RidePal.Data.Models;
using RidePal.Services.DTOModels;
using RidePal.Services.Models;

namespace RidePal.Services.Interfaces
{
    public interface IBingMapsServices
    {
        Task<LocationPoint> GetLocation(string countryRegion, string adminDistinct, string addressLine);

        Task<TripDTO> GetTrip(TripQuerryParameters parameters);
    }
}
