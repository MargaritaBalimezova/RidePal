using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RidePal.Web.MappingConfig;
using RidePal.Data;
using RidePal.Services.Interfaces;
using System;
using System.Threading.Tasks;
using RidePal.Services.DTOModels;
using RidePal.Services.Services;
using RidePal.Tests.Helpers;

namespace RidePal.Tests.UserServiceTests
{
    [TestClass]
    public class PostAsyncTests
    {
        private static IMapper _mapper;
        private RidePalContext context;
        private readonly IEmailService emailService;
        private readonly IConfiguration configuration;

        public PostAsyncTests()
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
        public async Task PostAsync_Should_CreateUser()
        {
            await context.AddRangeAsync(Seed.Users);
            await context.SaveChangesAsync();

            var user = new UpdateUserDTO
            {
                Username = "fooBarUsername",
                FirstName = "Patkan",
                LastName = "Slavchev",
                Email = "foobarmail@gmail.com",
                Password = "1234asdfgh",
            };

            var numberOfUsers = Seed.Users.Count;

            var service = new UserServices(emailService, configuration, context, _mapper);

            var actual = await service.PostAsync(user);

            Assert.IsTrue(actual.Username == user.Username);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task PostAsync_Should_ThrowException_When_EmailExists()
        {
            await context.AddRangeAsync(Seed.Users);
            await context.SaveChangesAsync();

            var user = new UpdateUserDTO
            {
                Username = "fooBarUsername",
                FirstName = "Patkan",
                LastName = "Slavchev",
                Email = "fakeemail@gmail.com",
                Password = "1234asdfgh",
            };

            var service = new UserServices(emailService, configuration, context, _mapper);

            await service.PostAsync(user);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task PostAsync_Should_ThrowException_When_UsernameExists()
        {
            await context.AddRangeAsync(Seed.Users);
            await context.SaveChangesAsync();

            var user = new UpdateUserDTO
            {
                Username = "AngelMarinski",
                FirstName = "Patkan",
                LastName = "Slavchev",
                Email = "mailmeangelo@gmail.com",
                Password = "1234asdfgh",
            };

            var service = new UserServices(emailService, configuration, context, _mapper);

            await service.PostAsync(user);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task PostAsync_Should_ThrowException_When_UsernameLenghtIsLess()
        {
            await context.AddRangeAsync(Seed.Users);
            await context.SaveChangesAsync();

            var user = new UpdateUserDTO
            {
                Username = "foo",
                FirstName = "Patkan",
                LastName = "Slavchev",
                Email = "foobarmail@gmail.com",
                Password = "1234asdfgh",
            };

            var service = new UserServices(emailService, configuration, context, _mapper);

            await service.PostAsync(user);
        }
    }
}