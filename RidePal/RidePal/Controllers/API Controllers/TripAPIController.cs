using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RidePal.Services.Exceptions;
using RidePal.Services.Interfaces;
using RidePal.Services.Models;
using System;
using System.Threading.Tasks;

namespace RidePal.WEB.Controllers.API_Controllers
{
    [Route("api/trips")]
    [ApiController]
    public class TripAPIController : ControllerBase
    {
        private readonly ITripServices tripServices;
        private readonly IBingMapsServices mapsServices;

        public TripAPIController(ITripServices tripServices, IBingMapsServices mapsServices)
        {
            this.tripServices = tripServices;
            this.mapsServices = mapsServices;
        }


        [HttpGet("")]
        public IActionResult GetTrips()
        {
            try
            {
                var trips = tripServices.Get();
                return Ok(trips);
            }
            catch (EntityNotFoundException ex)
            {

                return NotFound(ex.Message);
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetTripById(int id)
        {
            try
            {
                var trip = await tripServices.GetByIdAsync(id);
                return Ok(trip);
            }
            catch (EntityNotFoundException ex)
            {

                return NotFound(ex.Message);
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTripAsync(int id)
        {
            try
            {
                var trip = await tripServices.DeleteAsync(id);
                return Ok(trip);
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }


        [HttpPost("/trips")]
        public async Task<IActionResult> PostTripAsync(TripQuerryParameters tripParameters)
        {

            try
            {
                var tripDTO = await this.mapsServices.GetTrip(tripParameters);
                var trip = await this.tripServices.PostAsync(tripDTO);
                return Ok(trip);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTripAsync(int id,TripQuerryParameters tripParameters)
        {
            

            try
            {
                var trip = await this.mapsServices.GetTrip(tripParameters);
                var res = await this.tripServices.UpdateAsync(id, trip);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
