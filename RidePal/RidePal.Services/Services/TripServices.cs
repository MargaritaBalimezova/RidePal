using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RidePal.Data;
using RidePal.Data.Models;
using RidePal.Services.DTOModels;
using RidePal.Services.Exceptions;
using RidePal.Services.Helpers;
using RidePal.Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RidePal.Services.Services
{
    public class TripServices : ITripServices
    {
        private readonly RidePalContext context;
        private readonly IMapper mapper;

        public TripServices(RidePalContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<TripDTO> DeleteAsync(int id)
        {
            var trip = await this.context.Trips.FirstOrDefaultAsync(t => t.Id == id && t.IsDeleted == false) ?? throw new EntityNotFoundException(Constants.TRIP_NOT_FOUND);

            trip.IsDeleted = true;
            trip.DeletedOn = DateTime.Now;

            await context.SaveChangesAsync();

            return this.mapper.Map<TripDTO>(trip);
        }

        public IQueryable<TripDTO> Get()
        {
            var trips = this.context.Trips.Where(t => t.IsDeleted == false).Select(x => this.mapper.Map<TripDTO>(x)).AsQueryable();

            if (trips.Count() == 0)
            {
                throw new EntityNotFoundException(Constants.NO_TRIPS_FOUND);
            }

            return trips;
        }

        public async Task<TripDTO> GetByIdAsync(int id)
        {
            var trip = await this.context.Trips.FirstOrDefaultAsync(t => t.Id == id && t.IsDeleted == false) ?? throw new EntityNotFoundException(Constants.TRIP_NOT_FOUND);

            return this.mapper.Map<TripDTO>(trip);
        }

        public async Task<TripDTO> PostAsync(TripDTO obj)
        {
            var trip = new Trip
            {
                StartPoint = obj.StartPoint,
                Destination = obj.Destination,
                Distance = obj.Distance,
                Duration = obj.Duration,
                IsDeleted = false
            };

            if (trip == null)
            {
                throw new InvalidOperationException(Constants.NOT_ENOUGH_PARAMETERS_PASSED);
            }
            context.Trips.Add(trip);

            await context.SaveChangesAsync();

            return this.mapper.Map<TripDTO>(trip);
        }

        public async Task<TripDTO> UpdateAsync(int id, TripDTO obj)
        {
            var trip = await this.context.Trips.FirstOrDefaultAsync(t => t.Id == id && t.IsDeleted == false) ?? throw new EntityNotFoundException(Constants.TRIP_NOT_FOUND);

            if (obj.Destination != null && obj.Distance != 0 && obj.Duration != 0 && obj.StartPoint != null)
            {
                trip.Destination = obj.Destination;
                trip.Distance = obj.Distance;
                trip.StartPoint = obj.StartPoint;
                trip.Duration = obj.Duration;
            }
            else
            {
                throw new InvalidOperationException(Constants.NOT_ENOUGH_PARAMETERS_PASSED);
            }

            await context.SaveChangesAsync();

            return this.mapper.Map<TripDTO>(trip);
        }
    }
}