using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RidePal.Data;
using RidePal.Services.DTOModels;
using RidePal.Services.Services;
using RidePal.Tests.Helpers;
using RidePal.Web.MappingConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RidePal.Tests.TripServicesTests
{
    [TestClass]
    public class PostTripTests
    {

        private static IMapper mapper;
        private RidePalContext context;

        public PostTripTests()
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
        [ExpectedException(typeof (InvalidOperationException))]

        public async Task PostTripAsync_Should_Throw_When_InvalidModelIsPassed()
        {
            await context.AddRangeAsync(Seed.Trips);

            await context.SaveChangesAsync();

            var service = new TripServices(context, mapper);

            TripDTO trip = null;

            await service.PostAsync(trip);

        }

        [TestMethod]

        public async Task PostTripAsync_Should_Post_When_ValidModelIsPassed()
        {
            await context.AddRangeAsync(Seed.Trips);

            await context.SaveChangesAsync();

            var service = new TripServices(context, mapper);

            var currCount = Seed.Trips.Count;
            TripDTO trip = new TripDTO
            {
                StartCoordinates = "43.269722,26.8915296,12",
                DestinationCoordinates = "42.8948797,23.3691453,8.39",
                Duration = 202,
                Distance = 291,
                Destination = "Шумен,България",
                StartPoint = "Варна,България"
            };

            await service.PostAsync(trip);

            Assert.AreEqual(currCount + 1, context.Trips.Count());

        }

    }
}
