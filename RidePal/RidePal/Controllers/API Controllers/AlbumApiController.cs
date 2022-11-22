using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RidePal.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RidePal.WEB.Controllers.API_Controllers
{
    [Route("api/albums")]
    [ApiController]
    public class AlbumAPIController : ControllerBase
    {
        private readonly IAlbumService albumService;
        private readonly IGenreService genreService;

        public AlbumAPIController(IAlbumService albumService, IGenreService genreService)
        {
            this.albumService = albumService;
            this.genreService = genreService;
        }

        [HttpGet("genres/{genre}")]
        public async Task<IActionResult> GetAlbumByGenreAsync(string genre)
        {
            try
            {
                var genreDTO = await this.genreService.GetGenreByName(genre);
                var albums = await this.albumService.GetAlbumByGenreAsync(genreDTO);

                return this.Ok(albums);
            }
            catch(Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAlbumByIdAsync(int id)
        {
            try
            {
                var album = await this.albumService.GetAlbumByIdAsync(id);

                return this.Ok(album);
            }
            catch(Exception ex)
            {
                return this.NotFound(ex.Message);
            }
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAlbums()
        {
            try
            {
                var albums = await this.albumService.GetAlbums().ToListAsync();

                return this.Ok(albums);
            }
            catch(Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
    }
}
