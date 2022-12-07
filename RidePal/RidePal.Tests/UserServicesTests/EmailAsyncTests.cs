using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RidePal.Data;
using RidePal.Services.Interfaces;
using RidePal.Services.Models;
using RidePal.Services.Services;
using RidePal.Tests.Helpers;
using RidePal.Web.MappingConfig;
using System;
using System.Threading.Tasks;

namespace RidePal.Tests.UserServicesTests
{
    [TestClass]
    public class EmailAsyncTests
    {
        private static IMapper _mapper;
        private RidePalContext context;
        private readonly IEmailService emailService;
        private readonly IConfiguration configuration;

        public EmailAsyncTests()
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

            if (configuration == null)
            {
                var builder = new ConfigurationBuilder().AddJsonFile($"appsettings.json", optional: false);
                configuration = builder.Build();
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
        public async Task ResetPasswordAsync_Should_RerturnTrueWhenUserExists()
        {
            await context.AddRangeAsync(Seed.Users);
            await context.SaveChangesAsync();

            var service = new UserServices(emailService, configuration, context, _mapper);

            var user = new ResetPasswordModel
            {
                UserId = "1",
                NewPassword = "123456789",
                ConfirmNewPassword = "123456789",
            };

            Assert.AreEqual(true, await service.ResetPasswordAsync(user));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task ResetPasswordAsync_Should_ThrowExceptionWhenUserDoesntExist()
        {
            await context.AddRangeAsync(Seed.Users);
            await context.SaveChangesAsync();

            var service = new UserServices(emailService, configuration, context, _mapper);

            var user = new ResetPasswordModel
            {
                UserId = "0",
                NewPassword = "123456789",
                ConfirmNewPassword = "123456789",
            };

            await service.ResetPasswordAsync(user);
        }

        [TestMethod]
        public async Task ConfirmEmailAsync_Should_RerturnTrueWhenUserExists()
        {
            await context.AddRangeAsync(Seed.Users);
            await context.SaveChangesAsync();

            var service = new UserServices(emailService, configuration, context, _mapper);

            var userId = "1";

            Assert.AreEqual(true, await service.ConfirmEmailAsync(userId, "asu231"));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task ConfirmEmailAsync_Should_ThrowExceptionWhenUserDoesntExists()
        {
            await context.AddRangeAsync(Seed.Users);
            await context.SaveChangesAsync();

            var service = new UserServices(emailService, configuration, context, _mapper);

            var userId = "0";

            await service.ConfirmEmailAsync(userId, "asu231");
        }
    }
}