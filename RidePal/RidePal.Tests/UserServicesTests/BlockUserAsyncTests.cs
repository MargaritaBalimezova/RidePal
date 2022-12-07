using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RidePal.Tests;
using RidePal.Web.MappingConfig;
using RidePal.Data;
using RidePal.Services.Interfaces;
using RidePal.Services.Services;
using RidePal.Tests.Helpers;
using System;
using System.Threading.Tasks;

namespace RidePal.Tests.UserServiceTests
{
    [TestClass]
    public class BlockUserAsyncTests
    {
        private static IMapper _mapper;
        private RidePalContext context;
        private readonly IEmailService emailService;
        private readonly IConfiguration configuration;

        public BlockUserAsyncTests()
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

            RidePalContext movieForumContext = new RidePalContext(options);
            context = movieForumContext;
        }

        [TestMethod]
        public async Task BlockUser_Should_BlockUserByEmail()
        {
            await context.AddRangeAsync(Seed.Users);
            await context.SaveChangesAsync();

            var userEmail = "fakeemail@gmail.com";

            var service = new UserServices(emailService, configuration, context, _mapper);

            await service.BlockUserAsync(userEmail);

            Assert.IsTrue(context.Users.FirstOrDefaultAsync(x => x.Email == userEmail).Result.IsBlocked == true);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task BlockUser_Should_ThrowException_When_UserIsBlocked()
        {
            await context.AddRangeAsync(Seed.Users);
            await context.SaveChangesAsync();

            var userEmail = "blockeduser@gmail.com";

            var service = new UserServices(emailService, configuration, context, _mapper);

            await service.BlockUserAsync(userEmail);
        }

        [TestMethod]
        public async Task UnblockUser_Should_UnblockUserByEmail()
        {
            await context.AddRangeAsync(Seed.Users);
            await context.SaveChangesAsync();

            var userEmail = "blockeduser@gmail.com";

            var service = new UserServices(emailService, configuration, context, _mapper);

            await service.UnblockUserAsync(userEmail);

            Assert.IsTrue(context.Users.FirstOrDefaultAsync(x => x.Email == userEmail).Result.IsBlocked == false);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task UnblockUser_Should_ThrowException_When_UserIsUnblocked()
        {
            await context.AddRangeAsync(Seed.Users);
            await context.SaveChangesAsync();

            var userEmail = "fakeemail@gmail.com";

            var service = new UserServices(emailService, configuration, context, _mapper);

            await service.UnblockUserAsync(userEmail);
        }
    }
}