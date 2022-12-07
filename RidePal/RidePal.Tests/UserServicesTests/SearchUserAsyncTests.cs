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
using System.Linq;
using System.Threading.Tasks;

namespace RidePal.Tests.UserServicesTests
{
    [TestClass]
    public class SearchUserAsyncTests
    {
        private static IMapper _mapper;
        private RidePalContext context;
        private readonly IEmailService emailService;
        private readonly IConfiguration configuration;

        public SearchUserAsyncTests()
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
        public async Task SearchUsersAsync_Should_Return_UsersMatchingTheUsername()
        {
            //Arrange
            await context.AddRangeAsync(Seed.Users);

            await context.SaveChangesAsync();

            var service = new UserServices(emailService, configuration, context, _mapper);

            var userSearch = "magg";
            int type = 1;

            //Act
            var actual = await service.SearchAsync(userSearch, type);
            var expected = Seed.Users.Where(x => (x.Username.Contains(userSearch) && x.IsDeleted == false));

            //Assert
            Assert.AreEqual(expected.Count(), actual.Count());
        }

        [TestMethod]
        public async Task SearchUsersAsync_Should_Return_UsersContainingTheEmail()
        {
            //Arrange
            await context.AddRangeAsync(Seed.Users);

            await context.SaveChangesAsync();

            var service = new UserServices(emailService, configuration, context, _mapper);

            var userSearch = "@gmail.com";
            int type = 2;

            //Act
            var actual = await service.SearchAsync(userSearch, type);
            var expected = Seed.Users.Where(x => (x.Email.Contains(userSearch) && x.IsDeleted == false));

            //Assert
            Assert.AreEqual(expected.Count(), actual.Count());
        }

        [TestMethod]
        public async Task SearchUsersAsync_Should_Return_UsersContainingTheFirstName()
        {
            //Arrange
            await context.AddRangeAsync(Seed.Users);

            await context.SaveChangesAsync();

            var service = new UserServices(emailService, configuration, context, _mapper);

            var userSearch = "a";
            int type = 3;

            //Act
            var actual = await service.SearchAsync(userSearch, type);
            var expected = Seed.Users.Where(x => (x.FirstName.Contains(userSearch) && x.IsDeleted == false));

            //Assert
            Assert.AreEqual(expected.Count(), actual.Count());
        }

        [TestMethod]
        public async Task SearchUsersAsync_Should_Return_AllUsersWhenEmpty()
        {
            //Arrange
            await context.AddRangeAsync(Seed.Users);

            await context.SaveChangesAsync();

            var service = new UserServices(emailService, configuration, context, _mapper);

            var userSearch = "";
            int type = 3;

            //Act
            var actual = await service.SearchAsync(userSearch, type);
            var expected = Seed.Users.Where(x => x.IsDeleted == false);

            //Assert
            Assert.AreEqual(expected.Count(), actual.Count());
        }
    }
}