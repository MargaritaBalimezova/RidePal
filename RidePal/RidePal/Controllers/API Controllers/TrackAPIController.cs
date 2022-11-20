using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using RidePal.Data.Models;
using RidePal.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RidePal.WEB.Controllers.API_Controllers
{
    [Route("api/tracks")]
    [ApiController]
    public class TrackAPIController : ControllerBase
    {
        private readonly ITrackServices trackServices;
        public TrackAPIController(ITrackServices trackServices)
        {
            this.trackServices = trackServices;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            try
            {
                var tracks = this.trackServices.GetTracksSortedByRankDesc();

                return this.Ok(tracks);
            }
            catch(Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        [HttpGet("{genre}")]
        public async Task<IActionResult> GetTracksByGenre(string genreStr)
        {
            try
            {
                var tracks = await this.trackServices.GetTracksByGenreName(new Genre { Name = genreStr, IsDeleted = false });

                return this.Ok(tracks);
            }
            catch(Exception ex)
            {
                return this.NotFound(ex.Message);
            }
        }
    }
}
