using RidePal.Services.DTOModels;
using System.Threading.Tasks;

namespace RidePal.Services.Interfaces
{
    public interface ITripServices : ICRUDOperations<TripDTO>
    {
        Task<TripDTO> GetByIdAsync(int id);
    }
}