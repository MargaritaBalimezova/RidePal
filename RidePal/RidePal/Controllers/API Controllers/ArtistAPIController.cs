using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RidePal.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RidePal.WEB.Controllers.API_Controllers
{
    [Route("api/artists")]
    [ApiController]
    public class ArtistAPIController : ControllerBase
    {
        private readonly IArtistService artistService;

        public ArtistAPIController(IArtistService artistService)
        {
            this.artistService = artistService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetArtistByIdAsync(int id)
        {
            try
            {
                var artist = await this.artistService.GetArtistByIdAsync(id);

                return this.Ok(artist);
            }
            catch (Exception ex)
            {
                return this.NotFound(ex.Message);
            }
        }

        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetArtistByNameAsync(string name)
        {
            try
            {
                var artist = await this.artistService.GetArtistByNameAsync(name);

                return this.Ok(artist);
            }
            catch (Exception ex)
            {
                return this.NotFound(ex.Message);
            }
        }

        [HttpGet("{id}/albums")]
        public async Task<IActionResult> GetArtistAlbumsByArtistAsync(int id)
        {
            try
            {
                var albums = await this.artistService.GetArtistAlbumsByArtistAsync(id);

                return this.Ok(albums);
            }
            catch (Exception ex)
            {
                return this.NotFound(ex.Message);
            }
        }

        [HttpGet("")]
        public async Task<IActionResult> GetArtistsAsync()
        {
            try
            {
                return this.Ok(await this.artistService.GetArtistsAsync());
            }
            catch(Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}/tracks")]
        public async Task<IActionResult> GetArtistTracks(int id)
        {
            try
            {
                var tracks = await this.artistService.GetArtistTracks(id);

                return this.Ok(tracks);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
    }
}
