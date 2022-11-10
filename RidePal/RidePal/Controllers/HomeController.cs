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

        public HomeController(ILogger<HomeController> logger, IBingMapsServices mapsService)
        {
            _logger = logger;
            _mapsService = mapsService;
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

            var res = await  _mapsService.GetTrip(cred);

            return View(res);
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
