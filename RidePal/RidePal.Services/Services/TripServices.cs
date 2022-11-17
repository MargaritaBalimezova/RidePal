using RidePal.Data;
using RidePal.Services.DTOModels;
using RidePal.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RidePal.Services.Exceptions;
using RidePal.Services.Helpers;
using AutoMapper;
using RidePal.Data.Models;

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
            var trip = this.context.Trips.FirstOrDefault(t => t.Id == id && t.IsDeleted!=false) ?? throw new EntityNotFoundException(Constants.TRIP_NOT_FOUND);

            trip.IsDeleted = true;
            trip.DeletedOn = DateTime.Now;

            await context.SaveChangesAsync();

            return this.mapper.Map<TripDTO>(trip);
        }

        public IQueryable<TripDTO> Get()
        {
            var trips = this.context.Trips.Where(t => t.IsDeleted == false).ToList();

            return this.mapper.Map<IQueryable<TripDTO>>(trips);
        }

        public async Task<TripDTO> PostAsync(TripDTO obj)
        {
            var trip = new Trip
            {
                StartPoint = obj.StartPoint,
                Destination = obj.Destination,
                Distance = obj.Distance,
                Duration = obj.Duration,
                PlaylistId = obj.PlaylistId,
                IsDeleted = false
            };

            context.Trips.Add(trip);

            await context.SaveChangesAsync();

            return this.mapper.Map<TripDTO>(trip);
        }

        public async Task<TripDTO> UpdateAsync(int id, TripDTO obj)
        {
            var trip = this.context.Trips.FirstOrDefault(t => t.Id == id && t.IsDeleted != false) ?? throw new EntityNotFoundException(Constants.TRIP_NOT_FOUND);

            if (obj.Destination != null)
            {
                trip.Destination = obj.Destination;
            }
            if (obj.StartPoint != null)
            {
                trip.StartPoint = obj.StartPoint;
            }

            await context.SaveChangesAsync();

            return this.mapper.Map<TripDTO>(trip);

        }
    }
}
