using Microsoft.AspNetCore.Mvc;
using RidePal.Data.Models;
using RidePal.Models;
using RidePal.Services.Interfaces;
using RidePal.WEB.Models;
using System;
using System.Threading.Tasks;

namespace RidePal.WEB.Controllers
{
    public class GenresController : Controller
    {
        private const int TOP100 = 15;
        private readonly IGenreService genreService;
        private readonly ITrackServices trackServices;

        public GenresController(IGenreService genreService, ITrackServices trackServices)
        {
            this.genreService = genreService;
            this.trackServices = trackServices;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string name)
        {
            try
            {
                var genre = await this.genreService.GetGenreByName(name);
                var tracks = await this.trackServices.GetTopXTracksAsync(TOP100, new Genre { Name = genre.Name, Id = genre.Id, IsDeleted = false });

                return this.View(new GenreTopTracksWrapModel { Genre = genre, Tracks = tracks });
            }
            catch (Exception ex)
            {
                return this.View("Error", new ErrorViewModel
                {
                    RequestId = ex.Message
                });
            }
        }
    }
}
