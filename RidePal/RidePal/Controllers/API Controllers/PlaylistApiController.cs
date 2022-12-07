using Microsoft.AspNetCore.Mvc;
using RidePal.Services.DTOModels;
using RidePal.Services.Exceptions;
using RidePal.Services.Interfaces;
using RidePal.WEB.Models;
using System;
using System.Threading.Tasks;

namespace RidePal.WEB.Controllers.API_Controllers
{
    [Produces("application/json")]

    [Route("api/playlists")]
    [ApiController]
    public class PlaylistAPIController : ControllerBase
    {
        private readonly IPlaylistServices playlistServices;

        public PlaylistAPIController(IPlaylistServices playlistServices)
        {
            this.playlistServices = playlistServices;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var playlists = await this.playlistServices.GetAsync();

                return this.Ok(playlists);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("{title}")]
        public async Task<IActionResult> DeletePlaylist(string title)
        {
            try
            {
                var playlist = await this.playlistServices.DeleteAsync(title);
                return this.Ok(playlist);
            }
            catch (Exception ex)
            {
                return this.NotFound(ex.Message);
            }
        }

        [HttpPut]
        [Route("update/playlist/{id}")]
        public async Task<IActionResult> UpdatePlaylist(int id, [FromForm] CreatePlaylistViewModel playlist)
        {
            try
            {
                var updatePlaylist = new UpdatePlaylistDTO
                {
                    Name = playlist.Name
                };

                var newPlaylist = await playlistServices.UpdateAsync(id, updatePlaylist);
                return this.Ok(newPlaylist);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlaylistById(int id)
        {
            try
            {
                var track = await this.playlistServices.GetPlaylistDTOAsync(id);

                return this.Ok(track);
            }
            catch (EntityNotFoundException ex)
            {
                return this.NotFound(ex.Message);
            }
        }

        [HttpGet("/count")]
        public async Task<IActionResult> GetPlaylistCount()
        {
            try
            {
                var count = await this.playlistServices.PlaylistCount();

                return this.Ok(count);
            }
            catch (InvalidOperationException ex)
            {
                return this.NotFound(ex.Message);
            }
        }
    }
}