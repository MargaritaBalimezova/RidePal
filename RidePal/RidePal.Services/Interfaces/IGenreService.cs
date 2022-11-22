using RidePal.Services.DTOModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RidePal.Services.Interfaces
{
    public interface IGenreService
    {
        Task<GenreDTO> GetGenreByName(string GenreName);
        Task<GenreDTO> GetGenreById(int id);
    }
}
