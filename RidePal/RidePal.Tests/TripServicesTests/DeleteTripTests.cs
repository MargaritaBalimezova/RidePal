using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RidePal.Data;
using RidePal.Services.Exceptions;
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
    public class DeleteTripTests
    {

        private static IMapper mapper;
        private RidePalContext context;

        public DeleteTripTests()
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

        public async Task DeleteTripAsync_Should_DeleteTtrip_When_IdIsCorrect()
        {
            await context.AddRangeAsync(Seed.Trips);

            await context.SaveChangesAsync();

            var service = new TripServices(context, mapper);

            var currentTrips = Seed.Trips.Count();

            var trip = await service.DeleteAsync(1);

            Assert.AreEqual(currentTrips-1,context.Trips.Count());
        }

        [TestMethod]
        [ExpectedException(typeof(EntityNotFoundException))]
        public async Task DeleteTripAsync_Should_Throw_When_IdIsNotCorrect()
        {
            await context.AddRangeAsync(Seed.Trips);

            await context.SaveChangesAsync();

            var service = new TripServices(context, mapper);

            var trip = await service.DeleteAsync(100);
        }

    }
}
