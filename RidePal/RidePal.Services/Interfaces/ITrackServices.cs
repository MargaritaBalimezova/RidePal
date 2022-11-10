using RidePal.Services.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RidePal.Services.Interfaces
{
    public interface ITrackServices : ICRUDOperations<TrackDTO>
    {
        Task<IQueryable<TrackDTO>> GetByIdAsync();
    }
}
