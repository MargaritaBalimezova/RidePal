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
    public class SearchController : Controller
    {
        private readonly ISearchService searchService;

        public SearchController(ISearchService searchService)
        {
            this.searchService = searchService;
        }
        public async Task<IActionResult> Index(string name)
        {
            if(name != null)
            {
                this.ViewData["search"] = name;
            }
            try
            {
                var tracks = await this.searchService.SearchTracksAsync(name);
                var albums = await this.searchService.SearchAlbumssAsync(name);
                var users = await this.searchService.SearchUsersAsync(name);
                var artists = await this.searchService.SearchArtistsAsync(name);
                var playlists = await this.searchService.SearchPlaylistsAsync(name);

                return this.View(new SearchResultWrapper 
                { Tracks = tracks, Artists = artists, Albums = albums, Users = users, Playlists = playlists });
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
