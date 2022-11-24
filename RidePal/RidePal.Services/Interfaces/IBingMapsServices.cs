using RidePal.Services.DTOModels;
using RidePal.Services.Models;
using System.Threading.Tasks;

namespace RidePal.Services.Interfaces
{
    public interface IBingMapsServices
    {
        Task<LocationPoint> GetLocation(string countryRegion, string adminDistinct, string addressLine);

        Task<TripDTO> GetTrip(TripQuerryParameters parameters);
    }
}