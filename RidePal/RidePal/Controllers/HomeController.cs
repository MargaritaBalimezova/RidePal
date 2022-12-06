using Amazon.S3.Transfer;
using Amazon.S3;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RidePal.Data.DataInitialize;
using RidePal.Data.DataInitialize.Interfaces;
using RidePal.Data.Models;
using RidePal.Models;
using RidePal.Services.Interfaces;
using RidePal.Services.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3.Model;

namespace RidePal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly IBingMapsServices mapsService;
        private readonly ITrackServices trackServices;
        private readonly IPlaylistServices playServices;

        public HomeController(ILogger<HomeController> logger,
            IBingMapsServices mapsService, ITrackServices trackServices,IPlaylistServices playServices
           )
        {
            this.logger = logger;
            this.mapsService = mapsService;
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