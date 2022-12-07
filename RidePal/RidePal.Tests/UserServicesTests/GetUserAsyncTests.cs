using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RidePal.Data;
using RidePal.Data.Models;
using RidePal.Services.DTOModels;
using RidePal.Services.Interfaces;
using RidePal.Services.Services;
using RidePal.Tests.Helpers;
using RidePal.Web.MappingConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RidePal.Tests.UserServiceTests
{
    [TestClass]
    public class GetUserAsyncTests
    {
        private static IMapper _mapper;
        private RidePalContext context;
        private readonly IEmailService emailService;
        private readonly IConfiguration configuration;

        public GetUserAsyncTests()
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
        public async Task GetUserIdAsync_Should_GetUserById()
        {
            await context.AddRangeAsync(Seed.Users);
            await context.SaveChangesAsync();

            var userId = 1;

            var expected = Seed.Users.FirstOrDefault(x => x.Id == userId);

            var service = new UserServices(emailService, configuration, context, _mapper);

            var actual = await service.GetUserDTOAsync(userId);

            Assert.AreEqual(expected.Username, actual.Username);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task GetUserIdAsync_Should_ThrowException_When_InvalidID()
        {
            await context.AddRangeAsync(Seed.Users);
            await context.SaveChangesAsync();

            var userId = int.MinValue;

            var service = new UserServices(emailService, configuration, context, _mapper);

            await service.GetUserDTOAsync(userId);
        }

        [TestMethod]
        public async Task GetUserIdAsync_Should_GetUserByUsername()
        {
            await context.AddRangeAsync(Seed.Users);
            await context.SaveChangesAsync();

            var userId = 1;

            var expected = Seed.Users.FirstOrDefault(x => x.Id == userId);

            var service = new UserServices(emailService, configuration, context, _mapper);

            var actual = await service.GetUserDTOAsync(expected.Username);

            Assert.AreEqual(expected.Id, actual.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task GetUserIdAsync_Should_ThrowException_When_InvalidUsername()
        {
            await context.AddRangeAsync(Seed.Users);
            await context.SaveChangesAsync();

            var username = "dummytext";

            var service = new UserServices(emailService, configuration, context, _mapper);

            await service.GetUserDTOAsync(username);
        }

        [TestMethod]
        public async Task GetUserIdAsync_Should_GetUserByEmail()
        {
            await context.AddRangeAsync(Seed.Users);
            await context.SaveChangesAsync();

            var email = "adminsemail@gmail.com";

            var expected = Seed.Users.FirstOrDefault(x => x.Email == email);

            var service = new UserServices(emailService, configuration, context, _mapper);

            var actual = await service.GetUserDTOByEmailAsync(email);

            Assert.AreEqual(expected.Username, actual.Username);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task GetUserIdAsync_Should_ThrowException_When_InvalidEmail()
        {
            await context.AddRangeAsync(Seed.Users);
            await context.SaveChangesAsync();

            var email = "fOO@abv.bg";

            var service = new UserServices(emailService, configuration, context, _mapper);

            await service.GetUserDTOByEmailAsync(email);
        }

        [TestMethod]
        public async Task UserCount_Should_ReturnAmountOfUsers()
        {
            await context.AddRangeAsync(Seed.Users);
            await context.SaveChangesAsync();

            var service = new UserServices(emailService, configuration, context, _mapper);

            var expectedNum = Seed.Users.Where(x => x.IsDeleted == false).Count();
            var actualNum = service.UserCount();

            Assert.AreEqual(expectedNum, actualNum);
        }

        [TestMethod]
        public async Task GetUserAllPlaylistsByEmail_Should_ReturnAllUserPlaylists()
        {
            await context.AddRangeAsync(Seed.Users);
            await context.AddRangeAsync(Seed.Playlists);
            await context.SaveChangesAsync();

            var userEmail = "fakeemail@gmail.com";

            var expected = context.Users.FirstOrDefaultAsync(x => x.Email == userEmail).Result.Playlists;

            var service = new UserServices(emailService, configuration, context, _mapper);

            var actual = new List<PlaylistDTO>(await service.GetAllPlaylistsAsync(userEmail));

            Assert.AreEqual(expected.Count, actual.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task GetUserAllPlaylistsByEmail_Should_ThrowException()
        {
            await context.AddRangeAsync(Seed.Users);
            await context.SaveChangesAsync();

            var service = new UserServices(emailService, configuration, context, _mapper);

            await service.GetAllPlaylistsAsync("nonexistingemail@gmail.com");
        }

        [TestMethod]
        public async Task GetUserAllFriendsByEmail_Should_ReturnAllUserFriends()
        {
            await context.AddRangeAsync(Seed.Users);
            await context.AddRangeAsync(Seed.Playlists);
            await context.SaveChangesAsync();

            var userEmail = "fakeemail@gmail.com";

            var expected = context.Users.FirstOrDefaultAsync(x => x.Email == userEmail).Result.Friends;

            var service = new UserServices(emailService, configuration, context, _mapper);

            var actual = new List<UserDTO>(await service.GetAllFriendsAsync(userEmail));

            Assert.AreEqual(expected.Count, actual.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task GetUserAllFriendsByEmail_Should_ThrowException()
        {
            await context.AddRangeAsync(Seed.Users);
            await context.SaveChangesAsync();

            var service = new UserServices(emailService, configuration, context, _mapper);

            await service.GetAllFriendsAsync("nonexistingemail@gmail.com");
        }

        [TestMethod]
        public async Task GetUserAllFriendRequestsByEmail_Should_ReturnAllUserFriendRequests()
        {
            await context.AddRangeAsync(Seed.Users);
            await context.AddRangeAsync(Seed.Playlists);
            await context.SaveChangesAsync();

            var userEmail = "fakeemail@gmail.com";

            var expected = context.Users.FirstOrDefaultAsync(x => x.Email == userEmail).Result.ReceivedFriendRequests;

            var service = new UserServices(emailService, configuration, context, _mapper);

            var actual = new List<FriendRequest>(await service.GetAllFriendRequestsAsync(userEmail));

            Assert.AreEqual(expected.Count, actual.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task GetUserAllFriendRequestsByEmail_Should_ThrowException()
        {
            await context.AddRangeAsync(Seed.Users);
            await context.SaveChangesAsync();

            var service = new UserServices(emailService, configuration, context, _mapper);

            await service.GetAllFriendRequestsAsync("nonexistingemail@gmail.com");
        }

        [TestMethod]
        public async Task GetAsync_Should_ReturnAllUsers()
        {
            await context.AddRangeAsync(Seed.Users);
            await context.SaveChangesAsync();

            var expected = Seed.Users.ToList();

            var service = new UserServices(emailService, configuration, context, _mapper);

            var actual = new List<UserDTO>(await service.GetAsync());

            Assert.AreEqual(expected.Count, actual.Count);
        }
    }
}