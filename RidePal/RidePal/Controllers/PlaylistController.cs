using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using RidePal.Data.Models;
using RidePal.Models;
using RidePal.Services.DTOModels;
using RidePal.Services.Interfaces;
using RidePal.Services.Models;
using RidePal.WEB.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Threading.Tasks;

namespace RidePal.WEB.Controllers
{
    [Authorize]
    public class PlaylistController : Controller
    {
        private readonly IPlaylistServices playlistService;
        private readonly ITripServices tripService;
        private readonly IUserServices userService;
        private readonly IGenreService genreService;

        private readonly IMapper mapper;
        private readonly IBingMapsServices bingMapsService;

        public PlaylistController(IPlaylistServices playlistService, ITripServices tripService, IMapper mapper, IBingMapsServices bingMapsService, IUserServices userService, IGenreService genreService)
        {
            this.playlistService = playlistService;
            this.tripService = tripService;
            this.userService = userService;
            this.mapper = mapper;
            this.bingMapsService = bingMapsService;
            this.genreService = genreService;
        }

        public async Task<IActionResult> Index(string title)
        {

            try
            {
                var playList = await playlistService.GetPlaylistDTOAsync(title);
                return View(mapper.Map<PlaylistViewModel>(playList));
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
            ViewData["Audiences"] = new SelectList(await FillAudiences(), "Id", "Name");

            this.ViewData["StartPoint"] = "The start of your journey";
            this.ViewData["ArrivePoint"] = "Your journey's destination";
            
            return this.View(playlist);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePlaylist(CreatePlaylistViewModel model, TripQuerryParameters coordinates)
        {
            if (coordinates.StartPoint==null || coordinates.ArrivingDestination==null)
            {
                return this.View(model);
            }
          
            this.ViewData["StartPoint"] = coordinates.StartingDestination;
            this.ViewData["ArrivePoint"] = coordinates.ArrivingDestination;

            //TODO: Chack model state
            if (model.Name.Length<4 || model.AudienceId==0)
            {
                return this.View(model);
            }
            if (await playlistService.IsExistingAsync(model.Name))
            {
                this.ModelState.AddModelError("Name", "Playlist with this title already exists.");
                return this.View(model);
            }

            var tripDTO = await bingMapsService.GetTrip(coordinates);
            try
            {
             

                var audience = await playlistService.GetAudienceAsync(model.AudienceId);

                var trip = await tripService.PostAsync(tripDTO);

                var playlistDTO = new PlaylistDTO
                {
                    Name = model.Name,
                    RepeatArtists = model.RepeatArtists,
                    TopSongs = model.TopSongs,
                    GenresWithPercentages = model.GenresWithPercentages,
                    Audience = audience,
                    Author = await userService.GetUserDTOByEmailAsync(this.User.Identity.Name),
                    Trip = trip
                };

                var newPlaylist = await playlistService.PostAsync(playlistDTO);

                return RedirectToAction("Index", new { title = $"{newPlaylist.Name}" });
            }
            catch (Exception ex)
            {
                this.ModelState.AddModelError("Email", ex.Message);

                return this.View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string title)
        {
            try
            {
                await playlistService.DeleteAsync(title);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                return RedirectToPage("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditPlaylist(string title)
        {
            var playlist = await playlistService.GetPlaylistDTOAsync(title);
            ViewData["Audiences"] = new SelectList(await FillAudiences(), "Id", "Name");
            var update = new UpdatePlaylistViewModel
            {
                Name = playlist.Name,
                Id = playlist.Id,
            };
            return this.View(update);
        }

        [HttpPost]
        public async Task<IActionResult> EditPlaylist(UpdatePlaylistViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            try
            {
                var playlist = await playlistService.GetPlaylistDTOAsync(model.Id);

                if (await playlistService.IsExistingAsync(model.Name))
                {
                    this.ModelState.AddModelError("Name", "Playlist with this title already exists.");
                }

                var updatePlaylistDTO = new UpdatePlaylistDTO();

                updatePlaylistDTO.Name = model.Name;
                updatePlaylistDTO.Audience = await playlistService.GetAudienceAsync(model.AudienceId);

                await playlistService.UpdateAsync(playlist.Id, updatePlaylistDTO);
            }
            catch (Exception)
            {
                return this.View(model);
            }

            return this.RedirectToAction("EditPlaylist", "Playlist", new { title = model.Name });
        }

        private async Task FillGenres(CreatePlaylistViewModel model)
        {
            var genres = await genreService.GetGenres();
            foreach (var item in genres)
            {
                var tempGenre = new GenreWithPercentage();
                tempGenre.GenreName = item.Name;
                model.GenresWithPercentages.Add(tempGenre);
            }
        }

        private async Task<List<Audience>> FillAudiences()
        {
            var audiences = await genreService.GetAudiences();
            return audiences.ToList();
        }
    }
}