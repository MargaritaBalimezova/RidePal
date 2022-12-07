using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    public class DeleteUserAsyncTests
    {
        private static IMapper _mapper;
        private RidePalContext context;
        private readonly IEmailService emailService;
        private readonly IConfiguration configuration;

        public DeleteUserAsyncTests()
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
        public async Task DeleteAsync_Should_DeleteUserByEmail()
        {
            await context.AddRangeAsync(Seed.Users);
            await context.SaveChangesAsync();

            var userEmail = "morefakeemails@gmail.com";

            var service = new UserServices(emailService, configuration, context, _mapper);

            var expected = await service.GetUserDTOByEmailAsync(userEmail);

            var actual = await service.DeleteAsync(expected.Email);

            Assert.AreEqual(expected.Username, actual.Username);
            Assert.IsTrue(await context.Users.AnyAsync(x => x.Id == actual.Id
                                                        && x.IsDeleted == true));
        }
    }
}