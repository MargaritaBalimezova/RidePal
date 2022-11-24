using RidePal.Data.Models;
using RidePal.Services.DTOModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RidePal.Services.Interfaces
{
    public interface IGenreService
    {
        Task<GenreDTO> GetGenreByName(string GenreName);

        Task<GenreDTO> GetGenreById(int id);

        Task<IEnumerable<Genre>> GetGenres();

        Task<IEnumerable<Audience>> GetAudiences();
    }
}