using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RidePal.Data;
using RidePal.Services.DTOModels;
using RidePal.Services.Interfaces;
using RidePal.Services.Services;
using RidePal.Tests.Helpers;
using RidePal.Web.MappingConfig;
using System;
using System.Threading.Tasks;

namespace RidePal.Tests.UserServiceTests
{
    [TestClass]
    public class UpdateAsyncTests
    {
        private static IMapper _mapper;
        private RidePalContext context;
        private readonly IEmailService emailService;
        private readonly IConfiguration configuration;

        public UpdateAsyncTests()
        {
            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new RidePalProfile());
                });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
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
        public async Task UpdateAsync_Should_UpdateUserInfo()
        {
            await context.AddRangeAsync(Seed.Users);
            await context.SaveChangesAsync();

            var user = new UpdateUserDTO
            {
                Username = "fooBarUsername",
                FirstName = "Patkan",
                LastName = "Slavchev",
                Password = "1234asdfgh",
                Email = "fakeemail@gmail.com",
            };

            var service = new UserServices(emailService, configuration, context, _mapper);

            var actual = await service.UpdateAsync(1, user);

            Assert.IsTrue(actual.FirstName == user.FirstName);
            Assert.IsTrue(actual.LastName == user.LastName);
            Assert.IsTrue(actual.Email == user.Email);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task UpdateAsync_Should_ThrowException_When_EmailExists()
        {
            await context.AddRangeAsync(Seed.Users);
            await context.SaveChangesAsync();

            var user = new UpdateUserDTO
            {
                Username = "fooBarUsername",
                FirstName = "Patkan",
                LastName = "Slavchev",
                Email = Seed.Users[1].Email,
                Password = "1234asdfgh"
            };

            var service = new UserServices(emailService, configuration, context, _mapper);

            await service.UpdateAsync(1, user);
        }
    }
}