using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RidePal.Data.Models;
using RidePal.Models;
using RidePal.Services.Interfaces;
using RidePal.Services.Models;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace RidePal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBingMapsServices _mapsService;
        private readonly ITrackServices trackServices;
        private readonly IMapper _mapper;

        public HomeController(ILogger<HomeController> logger,
            IBingMapsServices mapsService, ITrackServices trackServices,
            IMapper mapper
           )
        {
            _mapper = mapper;
            _logger = logger;
            _mapsService = mapsService;
            this.trackServices = trackServices;
        }

        public async Task<IActionResult> Index(TripQuerryParameters trip)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            var cred = new TripQuerryParameters
            {
                DepartCountry = trip.DepartCountry,
                ArriveCountry = trip.ArriveCountry,
                DepartCity = trip.DepartCity,
                ArriveCity = trip.ArriveCity,
                DepartAddress = trip.DepartAddress,
                ArriveAddress = trip.ArriveAddress
            };

            this.ViewData["DepartCountry"] = trip.DepartCountry;
            this.ViewData["ArriveCountry"] = trip.ArriveCountry;
            this.ViewData["DepartCity"] = trip.DepartCity;
            this.ViewData["ArriveCity"] = trip.ArriveCity;
            this.ViewData["DepartAddress"] = trip.DepartAddress;
            this.ViewData["ArriveAddress"] = trip.ArriveAddress;

            var res = await _mapsService.GetTrip(cred);
            return this.View(res);
        }

        public IActionResult Tracks()
        {
            int hour = 1;
            int minutes = 34;
            int duration = hour * 3600 + minutes * 60 + 14;

            var res = this.trackServices.GetTracksWithDistinctArtists(new Genre { Id = 1, Name = "Rap" }, duration).Select(x => _mapper.Map<Track>(x));

            return this.View(res);
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