using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RidePal.Data.Models;
using RidePal.Models;
using RidePal.Services.Interfaces;
using RidePal.Services.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace RidePal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBingMapsServices _mapsService;
        private readonly ISpotifyAccountServices _spotifyAccount;
        private readonly ISpotifyServices _spotify;

        public HomeController(ILogger<HomeController> logger, 
            IBingMapsServices mapsService, 
            ISpotifyAccountServices spotifyAccount, 
            ISpotifyServices spotify)
        {
            _logger = logger;
            _mapsService = mapsService;
            _spotifyAccount = spotifyAccount;
            _spotify = spotify;
        }

        public async Task<IActionResult> Index()
        {


            var cred = new TripQuerryParameters
            {
                DepartCountry = "BG",
                ArriveCountry = "BG",
                DepartCity = "Plovdiv",
                ArriveCity = "Sofia"
            };
            var token = await _spotifyAccount.GetToken("1384de232d244717acb19e6f96f43c16", "93871af5c604451f95319223c52b882c");

            var search = await _spotify.GetRapSongs(token);

            

            return View(search.OrderByDescending(x=>x.Rank));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
