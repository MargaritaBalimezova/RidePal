using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MovieForum.Web.MappingConfig;
using RidePal.Data;
using RidePal.Services.DTOModels;
using RidePal.Services.Exceptions;
using RidePal.Services.Services;
using RidePal.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RidePal.Tests.ArtistServiceTests
{

    [TestClass]
    public class GetArtistTests
    {
        private static IMapper mapper;
        private RidePalContext context;

        public GetArtistTests()
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
        public async Task GetTopArtists_Should_Return_TopXArtist()
        {
            //Arrange
            await context.AddRangeAsync(Seed.Artists);
            await context.AddRangeAsync(Seed.Tracks);

            await context.SaveChangesAsync();

            var service = new ArtistServices(context, mapper);

            var top = 1;

            //Act
            var actual = await service.GetTopArtists(top);
            var expected = Seed.Tracks.OrderByDescending(x => x.Rank).First();

            //Assert
            Assert.AreEqual(expected.Id, actual.First().Id);
        }

        [TestMethod]
        public async Task GetArtistAlbumsByArtistAsync_Should_Return_ArtistsAlbums()
        {
            //Arrange
            await context.AddRangeAsync(Seed.Artists);
            await context.AddRangeAsync(Seed.Albums);

            await context.SaveChangesAsync();

            var service = new ArtistServices(context, mapper);

            var id = 1;

            //Act
            var actual = await service.GetArtistAlbumsByArtistAsync(id);
            var expected = mapper.Map<IEnumerable<AlbumDTO>>(Seed.Albums
                .Where(x => x.ArtistId == id));

            //Assert
            Assert.AreEqual(expected.Count(), actual.Count());
        }

        [TestMethod]
        [ExpectedException(typeof(EntityNotFoundException),
            "GetArtistAlbumsByArtistAsync failed to throw exception when artis id is invalid!")]
        public async Task GetArtistAlbumsByArtistAsync_Should_ThrowException_When_ArtistIdInvalid()
        {
            //Arrange
            await context.AddRangeAsync(Seed.Artists);

            await context.SaveChangesAsync();

            var service = new ArtistServices(context, mapper);

            var id = int.MaxValue;

            //Act && Assert
            await service.GetArtistAlbumsByArtistAsync(id);
        }

        [TestMethod]
        public async Task GetArtistByIdAsync_Should_Return_ArtistOnGiveId()
        {
            //Arrange
            await context.AddRangeAsync(Seed.Artists);

            await context.SaveChangesAsync();

            var service = new ArtistServices(context, mapper);

            var id = 1;

            //Act
            var actual = await service.GetArtistByIdAsync(id);
            var expected = Seed.Artists
                .FirstOrDefault(x => x.Id == id);

            //Assert
            Assert.AreEqual(expected.Id, actual.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(EntityNotFoundException),
    "GetArtistByIdAsync failed to throw exception when artis id is invalid!")]
        public async Task GetArtistByIdAsync_Should_ThrowException_When_UnexistingIdIsPassed()
        {
            //Arrange
            await context.AddRangeAsync(Seed.Artists);

            await context.SaveChangesAsync();

            var service = new ArtistServices(context, mapper);

            var id = int.MaxValue;

            //Act && Assert
            await service.GetArtistByIdAsync(id);
        }

        [TestMethod]
        public async Task GetArtistByNameAsync_Should_Return_ArtistByName()
        {
            //Arrange
            await context.AddRangeAsync(Seed.Artists);

            await context.SaveChangesAsync();

            var service = new ArtistServices(context, mapper);

            var name = "Drake";

            //Act
            var actual = await service.GetArtistByNameAsync(name);
            var expected = Seed.Artists
                .FirstOrDefault(x => x.Name == name);

            //Assert
            Assert.AreEqual(expected.Id, actual.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(EntityNotFoundException),
    "GetArtistByNameAsync failed to throw exception when artis id is invalid!")]
        public async Task GetArtistByNameAsync_Should_ThrowException_When_UnexistingNameIsPassed()
        {
            //Arrange
            await context.AddRangeAsync(Seed.Artists);

            await context.SaveChangesAsync();

            var service = new ArtistServices(context, mapper);

            var name = "unexistingname";

            //Act && Assert
            await service.GetArtistByNameAsync(name);
        }

        [TestMethod]
        public async Task GetArtistsAsync_Should_Return_AllArtists()
        {
            //Arrange
            await context.AddRangeAsync(Seed.Artists);

            await context.SaveChangesAsync();

            var service = new ArtistServices(context, mapper);

            //Act
            var actual = await service.GetArtistsAsync();
            var expected = Seed.Artists;

            //Assert
            Assert.AreEqual(expected.Count(), actual.Count());
        }

        [TestMethod]
        public async Task GetArtistTracksAsync_Should_Return_ArtistsTracks()
        {
            //Arrange
            await context.AddRangeAsync(Seed.Artists);
            await context.AddRangeAsync(Seed.Tracks);

            await context.SaveChangesAsync();

            var service = new ArtistServices(context, mapper);

            var id = 1;

            //Act
            var actual = await service.GetArtistTracksAsync(id);
            var expected = Seed.Tracks.Where(x => x.ArtistId == id);

            //Assert
            Assert.AreEqual(expected.Count(), actual.Count());
        }

        [TestMethod]
        [ExpectedException(typeof(EntityNotFoundException),
            "GetArtistAlbumsByArtistAsync failed to throw exception when artis id is invalid!")]
        public async Task GetArtistTracksAsync_Should_ThrowException_When_InvalidIdPassed()
        {
            //Arrange
            await context.AddRangeAsync(Seed.Artists);

            await context.SaveChangesAsync();

            var service = new ArtistServices(context, mapper);

            var id = int.MaxValue;

            //Act && Assert
            await service.GetArtistTracksAsync(id);
        }

        [TestMethod]
        public async Task GetArtistStyleAsync_Should_Return_GenreName()
        {
            //Arrange
            await context.AddRangeAsync(Seed.Artists);
            await context.AddRangeAsync(Seed.Tracks);
            await context.AddRangeAsync(Seed.Genres);

            await context.SaveChangesAsync();

            var service = new ArtistServices(context, mapper);

            var id = 1;

            //Act
            var actual = await service.GetArtistStyleAsync(id);
            var genreId = Seed.Tracks.Where(x => x.ArtistId == id)
                                     .GroupBy(i => i.GenreId)
                                     .OrderByDescending(grp => grp.Count())
                                     .Select(grp => grp.Key).First();
            var expected = Seed.Genres.FirstOrDefault(x => x.Id == genreId)
                                    .Name;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(EntityNotFoundException),
            "GetArtistStyleAsync failed to throw exception when artis id is invalid!")]
        public async Task GetArtistStyleAsync_Should_ThrowException_When_InvalidIdPassed()
        {
            //Arrange
            await context.AddRangeAsync(Seed.Artists);

            await context.SaveChangesAsync();

            var service = new ArtistServices(context, mapper);

            var id = int.MaxValue;

            //Act && Assert
            await service.GetArtistStyleAsync(id);
        }

        [TestMethod]
        public async Task GetArtistTopTracksAsync_Should_Return_GenreId()
        {
            //Arrange
            await context.AddRangeAsync(Seed.Artists);
            await context.AddRangeAsync(Seed.Tracks);

            await context.SaveChangesAsync();

            var service = new ArtistServices(context, mapper);

            var id = 1;

            //Act
            var actual = await service.GetArtistTopTracksAsync(id);
            var expected = Seed.Tracks.Where(x => x.ArtistId == id)
                                .OrderByDescending(x => x.Rank)
                                .Take(10);

            //Assert
            Assert.AreEqual(expected.Count(), actual.Count());
            Assert.AreEqual(expected.First().Id, actual.First().Id);
        }

        [TestMethod]
        [ExpectedException(typeof(EntityNotFoundException),
            "GetArtistTopTracksAsync failed to throw exception when artis id is invalid!")]
        public async Task GetArtistTopTracksAsync_Should_ThrowException_When_InvalidIdPassed()
        {
            //Arrange
            await context.AddRangeAsync(Seed.Artists);

            await context.SaveChangesAsync();

            var service = new ArtistServices(context, mapper);

            var id = int.MaxValue;

            //Act && Assert
            await service.GetArtistTopTracksAsync(id);
        }
    }
}
