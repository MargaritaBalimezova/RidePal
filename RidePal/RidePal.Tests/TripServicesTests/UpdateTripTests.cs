using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RidePal.Data;
using RidePal.Services.DTOModels;
using RidePal.Services.Exceptions;
using RidePal.Services.Services;
using RidePal.Tests.Helpers;
using RidePal.Web.MappingConfig;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RidePal.Tests.TripServicesTests
{
    [TestClass]
    public class UpdateTripTests
    {

        private static IMapper mapper;
        private RidePalContext context;

        public UpdateTripTests()
        {
            if (mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new RidePalProfile());
                });
                IMapper _mapper = mappingConfig.CreateMapper();
                mapper = _mapper;
            }
        }

        [TestInitialize]
        public void Init()
        {
            var options = new DbContextOptionsBuilder<RidePalContext>()
                               .UseInMemoryDatabase(Guid.NewGuid().ToString())
                               .Options;

            RidePalContext ridePalContext = new RidePalContext(options);
            context = ridePalContext;
        }

        [TestMethod]
        [ExpectedException(typeof(EntityNotFoundException))]

        public async Task UpdateAsync_Should_Throw_When_InvalidId()
        {
            await context.AddRangeAsync(Seed.Trips);

            await context.SaveChangesAsync();

            var service = new TripServices(context, mapper);

            TripDTO trip = new TripDTO
            {
                StartCoordinates = "43.269722,26.8915296,12",
                DestinationCoordinates = "42.8948797,23.3691453,8.39",
                Duration = 202,
                Distance = 291,
                Destination = "Шумен,България",
                StartPoint = "Варна,България"
            };
            await service.UpdateAsync(100, trip);

        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]

        public async Task UpdateAsync_Should_Throw_When_InvalidObjIsPassed()
        {
            await context.AddRangeAsync(Seed.Trips);

            await context.SaveChangesAsync();

            var service = new TripServices(context, mapper);

            TripDTO trip = new TripDTO
            {
                StartCoordinates = "43.269722,26.8915296,12",
                DestinationCoordinates = "42.8948797,23.3691453,8.39",
                Destination = "Шумен,България",
                StartPoint = "Варна,България"
            };

            await service.UpdateAsync(1, trip);

        }

        [TestMethod]
        public async Task UpdateAsync_Should_TUpdate_When_DataIsValid()
        {
            await context.AddRangeAsync(Seed.Trips);

            await context.SaveChangesAsync();

            var service = new TripServices(context, mapper);

            TripDTO trip = new TripDTO
            {
                StartCoordinates = "43.269722,26.8915296,12",
                DestinationCoordinates = "42.8948797,23.3691453,8.39",
                Duration = 202,
                Distance = 291,
                Destination = "Шумен,България",
                StartPoint = "Варна,България"
            };

            await service.UpdateAsync(1, trip);

            var tripI = await context.Trips.FirstOrDefaultAsync(x => x.Id == 1);

            Assert.AreEqual(trip.Destination, tripI.Destination);

        }


    }
}
