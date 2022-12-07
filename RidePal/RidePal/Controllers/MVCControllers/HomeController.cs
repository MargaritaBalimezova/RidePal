using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RidePal.Models;
using RidePal.Services.Interfaces;
using System.Diagnostics;
using System.Threading.Tasks;

namespace RidePal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITrackServices trackServices;
        private readonly IPlaylistServices playServices;

        public HomeController(ITrackServices trackServices, IPlaylistServices playServices)
        {
            this.trackServices = trackServices;
            this.playServices = playServices;
        }

        public async Task<IActionResult> Index()
        {

            var lists = await playServices.GetAsync();

            return this.View(lists);
        }

        public async Task<IActionResult> Tracks()
        {
            var res = await this.trackServices.GetTopXTracksAsync(100);

            return this.View(res);
        }

        public IActionResult About()
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