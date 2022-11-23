using AutoMapper;
using RidePal.Data.Models;
using RidePal.Data;
using RidePal.Services.DTOModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RidePal.Services.Interfaces
{
    public interface IPlaylistServices
    {
        Task<IEnumerable<PlaylistDTO>> GetAsync();

        Task<PlaylistDTO> PostAsync(PlaylistDTO obj);

        Task<PlaylistDTO> UpdateAsync(int id, UpdatePlaylistDTO obj);

        Task<PlaylistDTO> DeleteAsync(string title);

        Task<int> PlaylistCount();

        Task<bool> IsExistingAsync(string title);

        Task<PlaylistDTO> GetPlaylistDTOAsync(string title);

        Task<PlaylistDTO> GetPlaylistDTOAsync(int id);

        Task<IEnumerable<Genre>> GetGenres();
    }
}