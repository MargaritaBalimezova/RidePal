using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RidePal.Data;
using RidePal.Services.Exceptions;
using RidePal.Services.Services;
using RidePal.Tests.Helpers;
using RidePal.Web.MappingConfig;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RidePal.Tests.GenreServiceTests
{
    [TestClass]
    public class GetGenreTestAsync
    {
        private static IMapper mapper;
        private RidePalContext context;

        public GetGenreTestAsync()
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

            RidePalContext movieForumContext = new RidePalContext(options);
            context = movieForumContext;
        }

        [TestMethod]
        public async Task GetGenreById_Should_Return_Genre_When_IdValid()
        {
            //Arrange
            await context.AddRangeAsync(Seed.Genres);

            await context.SaveChangesAsync();

            var service = new GenreServices(context, mapper);

            var id = 1;

            //Act
            var actual = await service.GetGenreById(id);
            var expected = Seed.Genres.FirstOrDefault(x => x.Id == id);

            //Assert
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.Id, actual.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(EntityNotFoundException),
            "GetGenreById failed to throw exception when genre id is invalid!")]
        public async Task GetGenreById_Should_ThrowException_When_IdInvalid()
        {
            //Arrange
            await context.AddRangeAsync(Seed.Genres);

            await context.SaveChangesAsync();

            var service = new GenreServices(context, mapper);

            var id = int.MaxValue;

            //Act & Assert
            await service.GetGenreById(id);
        }

        [TestMethod]
        public async Task GetGenreByName_Should_Return_Genre_When_NameValid()
        {
            //Arrange
            await context.AddRangeAsync(Seed.Genres);

            await context.SaveChangesAsync();

            var service = new GenreServices(context, mapper);

            var name = "rap";

            //Act
            var actual = await service.GetGenreByName(name);
            var expected = Seed.Genres.FirstOrDefault(x => x.Name.ToLower() == name);

            //Assert
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.Id, actual.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(EntityNotFoundException),
            "GetGenreById failed to throw exception when genre id is invalid!")]
        public async Task GetGenreByName_Should_ThrowException_When_NameInvalid()
        {
            //Arrange
            await context.AddRangeAsync(Seed.Genres);

            await context.SaveChangesAsync();

            var service = new GenreServices(context, mapper);

            var name = "invalidname";

            //Act & Assert
            await service.GetGenreByName(name);
        }

        [TestMethod]
        public async Task GetGenres_Should_Return_AllGenres()
        {
            //Arrange
            await context.AddRangeAsync(Seed.Genres);

            await context.SaveChangesAsync();

            var service = new GenreServices(context, mapper);

            //Act
            var actual = await service.GetGenres();

            //Assert
            Assert.AreEqual(Seed.Genres.Count(), actual.Count());
        }

        [TestMethod]
        public async Task GetAudience_Should_Return_AllAudiences()
        {
            //Arrange
            await context.AddRangeAsync(Seed.Audiences);

            await context.SaveChangesAsync();

            var service = new GenreServices(context, mapper);

            //Act
            var actual = await service.GetAudiences();

            //Assert
            Assert.AreEqual(Seed.Audiences.Count(), actual.Count());
        }

    }
}
