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
    public class GetTripTests
    {
        private static IMapper mapper;
        private RidePalContext context;

        public GetTripTests()
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
        public async Task Get_Should_GetAllTrips()
        {
            //Arrange
            await context.AddRangeAsync(Seed.Trips);

            await context.SaveChangesAsync();

            var service = new TripServices(context, mapper);


            var count = Seed.Trips.Count();
            //Act
            var actual = service.Get();

            //Assert
            Assert.AreEqual(count, actual.Count());
        }

        [TestMethod]
        public async Task GetByIdAsync_Should_GetTripById()
        {
            //Arrange
            await context.AddRangeAsync(Seed.Trips);

            await context.SaveChangesAsync();

            var service = new TripServices(context, mapper);

            //Act
            var actual = await service.GetByIdAsync(2);

            //Assert
            Assert.AreEqual(253, actual.Duration);
        }

        [TestMethod]
        [ExpectedException(typeof (EntityNotFoundException))]
        public async Task GetById_Should_Throw_When_InvalidIdIsPassed()
        {
            //Arrange
            await context.AddRangeAsync(Seed.Trips);

            await context.SaveChangesAsync();

            var service = new TripServices(context, mapper);

            //Act
            var actual = await service.GetByIdAsync(100);
        }
    }
}
