using RidePal.Services.DTOModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RidePal.Services.Interfaces
{
    public interface ITripServices: ICRUDOperations<TripDTO>
    {
        Task<TripDTO> GetByIdAsync(int id);
    }
}
