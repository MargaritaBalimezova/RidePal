using Microsoft.AspNetCore.Mvc;
using RidePal.Models;
using RidePal.Services.Interfaces;
using RidePal.WEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RidePal.WEB.Controllers
{
    public class ArtistsController : Controller
    {
        private readonly IArtistService artistService;

        public ArtistsController(IArtistService artistService)
        {
            this.artistService = artistService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var artists = await this.artistService.GetTopArtists(50);

            return View(artists);
        }

        [HttpGet]
        public async Task<IActionResult> Artist(int id)
        {
            try
            {
                var artist = await this.artistService.GetArtistByIdAsync(id);
                var albums = await this.artistService.GetArtistAlbumsByArtistAsync(id);
                var tracks = await this.artistService.GetArtistTopTracksAsync(id);
                var style = await this.artistService.GetArtistStyleAsync(id);

                return this.View(new ArtistWrapModel {TopTracks = tracks, Albums = albums, Artist = artist, Style = style });
            }
            catch(Exception ex)
            {
                return this.View("Error", new ErrorViewModel
                {
                    RequestId = ex.Message
                });
            }
        }

        [Route("Artists/Top")]
        public async Task<IActionResult> Top()
        {
            try
            {
                var artists = await this.artistService.GetTopArtists(100);

                return this.View(artists);
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
