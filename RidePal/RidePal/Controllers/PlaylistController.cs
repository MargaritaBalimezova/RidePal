using Microsoft.AspNetCore.Mvc;
using RidePal.Data.Models;
using RidePal.Services.DTOModels;
using RidePal.Services.Models;
using RidePal.Web.Models;
using System.Threading.Tasks;
using System;
using AutoMapper;
using RidePal.Services.Interfaces;
using RidePal.WEB.Models;
using RidePal.Models;
using System.Collections.Generic;

namespace RidePal.WEB.Controllers
{
    public class PlaylistController : Controller
    {
        private readonly IPlaylistServices playlistService;
        private readonly ITripServices tripService;
        private readonly IMapper mapper;
        private readonly IBingMapsServices bingMapsService;

        public PlaylistController(IPlaylistServices playlistService, ITripServices tripService, IMapper mapper, IBingMapsServices bingMapsService)
        {
            this.playlistService = playlistService;
            this.tripService = tripService;
            this.mapper = mapper;
            this.bingMapsService = bingMapsService;
        }

        public async Task<IActionResult> Index(string title)
        {
            try
            {
                var playList = await playlistService.GetPlaylistDTOAsync(title);
                return View(mapper.Map<CreatePlaylistViewModel>(playList));
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> CreatePlaylist()
        {
            var playlist = new CreatePlaylistViewModel();
            playlist.GenresWithPercentages = new List<GenreWithPercentage>();
            await FillGenres(playlist);
            return this.View(playlist);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePlaylist(CreatePlaylistViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }
            if (await playlistService.IsExistingAsync(model.Name))
            {
                this.ModelState.AddModelError("Name", "Playlist with this title already exists.");
                return this.View(model);
            }
            try
            {
                var startingPoint = model.Trip.StartPoint.Split(", ");
                var destination = model.Trip.Destination.Split(", ");
                var cred = new TripQuerryParameters
                {
                    DepartCountry = startingPoint[0],
                    ArriveCountry = destination[0],
                    DepartCity = startingPoint[1],
                    ArriveCity = destination[1],
                    DepartAddress = startingPoint[2],
                    ArriveAddress = destination[2]
                };

                //var tmpTrip = await bingMapsService.GetTrip(cred);
                var tmpTrip = new TripDTO
                {
                    Destination = model.Trip.Destination,
                    StartPoint = model.Trip.StartPoint,
                    Duration = 180,
                    Distance = 1000
                };

                var playlistDTO = new PlaylistDTO
                {
                    Name = model.Name,
                    RepeatArtists = model.RepeatArtists,
                    TopSongs = model.TopSongs,
                    GenresWithPercentages = model.GenresWithPercentages,
                    Audience = model.Audience,
                    Trip = tmpTrip
                };

                var newPlaylist = await playlistService.PostAsync(playlistDTO);

                tmpTrip.PlaylistId = newPlaylist.Id;

                await tripService.PostAsync(tmpTrip);

                return RedirectToAction("Index", "Playlist", new { title = newPlaylist.Name });
            }
            catch (Exception ex)
            {
                this.ModelState.AddModelError("Email", ex.Message);

                return this.View(model);
            }
        }

        private async Task FillGenres(CreatePlaylistViewModel model)
        {
            var genres = await playlistService.GetGenres();
            foreach (var item in genres)
            {
                var tempGenre = new GenreWithPercentage();
                tempGenre.GenreName = item.Name;
                model.GenresWithPercentages.Add(tempGenre);
            }
        }
    }
}