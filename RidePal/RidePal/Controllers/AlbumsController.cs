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
    public class AlbumsController : Controller
    {
        private readonly IAlbumService albumService;

        public AlbumsController(IAlbumService albumService)
        {
            this.albumService = albumService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("Albums/{id:int}")]
        public async Task<IActionResult> Album(int id)
        {
            try
            {
                var album = await this.albumService.GetAlbumByIdAsync(id);
                var suggested = await this.albumService.GetSuggestedAlbums((int)album.GenreId, album.Id);

                return this.View(new SingleAlbumWrapperModel { Album = album, SuggestedAlbums = suggested});
            }
            catch(Exception ex)
            {
                return this.View("Error", new ErrorViewModel
                {
                    RequestId = ex.Message
                });
            }
        }
    }
}
