using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RidePal.Data;
using RidePal.Services.Interfaces;
using RidePal.Services.Services;
using RidePal.Tests.Helpers;
using RidePal.Web.MappingConfig;
using System;
using System.Threading.Tasks;

namespace RidePal.Tests.UserServicesTests
{
    [TestClass]
    public class IsExistingUserTest
    {
        private static IMapper _mapper;
        private RidePalContext context;
        private readonly IEmailService emailService;
        private readonly IConfiguration configuration;

        public IsExistingUserTest()
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
        public async Task IsExistingAsync_Should_ReturnTrue()
        {
            await context.AddRangeAsync(Seed.Users);
            await context.SaveChangesAsync();

            var userEmail = "fakeemail@gmail.com";

            var service = new UserServices(emailService, configuration, context, _mapper);

            var actual = await service.IsExistingAsync(userEmail);

            Assert.AreEqual(true, actual);
        }

        [TestMethod]
        public async Task IsExistingUsernameAsync_Should_ReturnTrue()
        {
            await context.AddRangeAsync(Seed.Users);
            await context.SaveChangesAsync();

            var username = "AngelMarinski";

            var service = new UserServices(emailService, configuration, context, _mapper);

            var actual = await service.IsExistingUsernameAsync(username);

            Assert.AreEqual(true, actual);
        }
    }
}