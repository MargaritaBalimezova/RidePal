using Microsoft.AspNetCore.Mvc;
using RidePal.Data.Models;
using RidePal.Services.DTOModels;
using RidePal.Services.Exceptions;
using RidePal.Services.Interfaces;
using RidePal.WEB.Models;
using System;
using System.Threading.Tasks;

namespace RidePal.WEB.Controllers.API_Controllers
{
    [Route("api/tracks")]
    [ApiController]
    public class TrackAPIController : ControllerBase
    {
        private readonly ITrackServices trackServices;
        private readonly IGenreService genreService;

        public TrackAPIController(ITrackServices trackServices, IGenreService genreService)
        {
            this.trackServices = trackServices;
            this.genreService = genreService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var tracks = await this.trackServices.GetAllTracksAsync();

                return this.Ok(tracks);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTrackById(int id)
        {
            try
            {
                var track = await this.trackServices.GetByIdAsync(id);

                return this.Ok(track);
            }
            catch (EntityNotFoundException ex)
            {
                return this.NotFound(ex.Message);
            }
        }

        [HttpGet("/genres/{genre}")]
        public async Task<IActionResult> GetTracksByGenre(string genre)
        {
            try
            {
                var gen = await this.genreService.GetGenreByName(genre);
                var tracks = await this.trackServices.GetTracksByGenreAsync(new Genre { Id = gen.Id, Name = gen.Name, IsDeleted = false });

                return this.Ok(tracks);
            }
            catch (EntityNotFoundException ex)
            {
                return this.NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return this.NotFound(ex.Message);
            }
        }

        [HttpGet("/playlist/distinct")]
        public async Task<IActionResult> GetTracksForPlaylistDistinct([FromQuery] GetTracksByGenreDurationModelWrapper param)
        {
            try
            {
                var genre = await this.genreService.GetGenreByName(param.Genre);
                var tracks = this.trackServices.GetTracksWithDistinctArtists(new Genre { Id = genre.Id, Name = genre.Name, IsDeleted = false }
                                                            , param.DurationInMinutes * 60);

                return this.Ok(tracks);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        [HttpGet("/playlist/tracks")]
        public async Task<IActionResult> GetTracksForPlaylist([FromQuery] GetTracksByGenreDurationModelWrapper param)
        {
            try
            {
                var genre = await this.genreService.GetGenreByName(param.Genre);
                var tracks = this.trackServices.GetTracks(new Genre { Id = genre.Id, Name = genre.Name, IsDeleted = false }
                                                            , param.DurationInMinutes * 60);

                return this.Ok(tracks);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        [HttpGet("/top/{x}")]
        public async Task<IActionResult> GetTopXTracksAsync(int x, [FromQuery] string genre)
        {
            try
            {
                GenreDTO genreDTO = null;
                if (genre != null)
                {
                    genreDTO = await this.genreService.GetGenreByName(genre);
                }

                var topTracks = await this.trackServices
                    .GetTopXTracksAsync(x, genreDTO == null ? null : new Genre { Id = genreDTO.Id, Name = genreDTO.Name, IsDeleted = false });

                return this.Ok(topTracks);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
    }
}