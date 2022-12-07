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
    public class FriendsAsyncTests
    {
        private static IMapper _mapper;
        private RidePalContext context;
        private readonly IEmailService emailService;
        private readonly IConfiguration configuration;

        public FriendsAsyncTests()
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
        public async Task SendFriendRequestAsync_Should_SendFriendReqest()
        {
            await context.AddRangeAsync(Seed.Users);
            await context.SaveChangesAsync();

            var senderEmail = "fakeemail@gmail.com";
            var recipientEmail = "adminsemail@gmail.com";

            var service = new UserServices(emailService, configuration, context, _mapper);

            try
            {
                await service.SendFriendRequestAsync(senderEmail, recipientEmail);
                Assert.IsTrue(true);
            }
            catch
            {
                Assert.IsTrue(false);
            }
        }

        [TestMethod]
        public async Task AcceptFriendRequestAsync_Should_AcceptFriendReqest()
        {
            await context.AddRangeAsync(Seed.Users);
            await context.AddRangeAsync(Seed.FriendRequests);
            await context.SaveChangesAsync();

            var senderEmail = "fakeemail@gmail.com";
            var recipientEmail = "adminsemail@gmail.com";

            var service = new UserServices(emailService, configuration, context, _mapper);

            try
            {
                await service.AcceptFriendRequestAsync(senderEmail, recipientEmail);
                Assert.IsTrue(true);
            }
            catch
            {
                Assert.IsTrue(false);
            }
        }

        [TestMethod]
        public async Task DeclineFriendRequestAsync_Should_DeclineFriendReqest()
        {
            await context.AddRangeAsync(Seed.Users);
            await context.AddRangeAsync(Seed.FriendRequests);
            await context.SaveChangesAsync();

            var senderEmail = "fakeemail@gmail.com";
            var recipientEmail = "adminsemail@gmail.com";

            var service = new UserServices(emailService, configuration, context, _mapper);

            try
            {
                await service.DeclineFriendRequestAsync(senderEmail, recipientEmail);
                Assert.IsTrue(true);
            }
            catch
            {
                Assert.IsTrue(false);
            }
        }

        [TestMethod]
        public async Task RemoveFriendAsync_Should_RemoveFriend()
        {
            await context.AddRangeAsync(Seed.Users);
            await context.SaveChangesAsync();

            var email = "fakeemail@gmail.com";
            var friendEmail = "adminsemail@gmail.com";

            var service = new UserServices(emailService, configuration, context, _mapper);

            try
            {
                await service.RemoveFriendAsync(email, friendEmail);
                Assert.IsTrue(true);
            }
            catch
            {
                Assert.IsTrue(false);
            }
        }
    }
}