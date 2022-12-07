using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RidePal.Data;
using RidePal.Data.Models;
using RidePal.Services.Exceptions;
using RidePal.Services.Services;
using RidePal.Tests.Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RidePal.Tests.TrackServiceTests
{
    [TestClass]
    public class GetTrackTestsAsync
    {
        private static IMapper mapper;
        private RidePalContext context;

        public GetTrackTestsAsync()
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
        public async Task GetTracks_Should_Return_AllTracks()
        {
            //Arrange
            await context.AddRangeAsync(Seed.Tracks);

            await context.SaveChangesAsync();

            var service = new TrackServices(context, mapper);

            //Act
            var actual = await service.Get().ToListAsync();
            var expected = Seed.Tracks;

            //Assert
            Assert.AreEqual(expected.Count(), actual.Count());
        }

        [TestMethod]
        public async Task GetByIdAsync_Should_Return_TrackById()
        {
            //Arrange
            await context.AddRangeAsync(Seed.Tracks);

            await context.SaveChangesAsync();

            var service = new TrackServices(context, mapper);

            var id = 1;

            //Act
            var actual = await service.GetByIdAsync(id);
            var expected = Seed.Tracks.FirstOrDefault(x => x.Id == id);

            //Assert
            Assert.AreEqual(expected.Id, actual.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(EntityNotFoundException),
            "GetByIdAsync failed to throw exception when artis id is invalid!")]
        public async Task GetByIdAsync_Should_ThrowException_When_ArtistIdInvalid()
        {
            //Arrange
            await context.AddRangeAsync(Seed.Tracks);

            await context.SaveChangesAsync();

            var service = new TrackServices(context, mapper);

            var id = int.MaxValue;

            //Act && Assert
            await service.GetByIdAsync(id);
        }

        [TestMethod]
        public async Task GetTracksWithDistinctArtists_Should_Return_TracksWithDistinctArtist()
        {
            //Arrange
            await context.AddRangeAsync(Seed.Tracks);
            await context.AddRangeAsync(Seed.Genres);
            await context.AddRangeAsync(Seed.Artists);

            await context.SaveChangesAsync();

            var service = new TrackServices(context, mapper);

            int duration = 1500;
            var genre = Seed.Genres.FirstOrDefault(x => x.Name == "Rap");

            //Act
            var actual = service.GetTracksWithDistinctArtists(genre, duration);

            //Assert
            Assert.IsTrue(actual.Sum(x => x.Duration) >= 1100 &&
                actual.Sum(x => x.Duration) <= 1800);
        }

        [TestMethod]
        public async Task GetTracks_Should_Return_TracksOnGivenDurationAndGenre()
        {
            //Arrange
            await context.AddRangeAsync(Seed.Tracks);
            await context.AddRangeAsync(Seed.Genres);

            await context.SaveChangesAsync();

            var service = new TrackServices(context, mapper);

            int duration = 1500;
            var genre = Seed.Genres.FirstOrDefault(x => x.Name == "Rap");

            //Act
            var actual = service.GetTracks(genre, duration);

            //Assert
            Assert.IsTrue(actual.Sum(x => x.Duration) >= 1200 &&
                actual.Sum(x => x.Duration) <= 1800);
        }

        [TestMethod]
        public async Task GetTracksByGenreAsync_Should_Return_AllTracksOnGivenGenre()
        {
            //Arrange
            await context.AddRangeAsync(Seed.Tracks);
            await context.AddRangeAsync(Seed.Genres);

            await context.SaveChangesAsync();

            var service = new TrackServices(context, mapper);

            var genre = Seed.Genres.FirstOrDefault(x => x.Name == "Rap");

            //Act
            var actual = await service.GetTracksByGenreAsync(genre);
            var expected = Seed.Tracks.Where(track => track.GenreId == genre.Id);

            //Assert
            Assert.AreEqual(expected.Count(), actual.Count());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException),
            "GetTracksByGenreAsync failed to throw exception when passed genre is invalid!")]
        public async Task GetTracksByGenreAsync_Should_ThrowException_When_GenreIsInvalid()
        {
            //Arrange
            await context.AddRangeAsync(Seed.Tracks);
            await context.AddRangeAsync(Seed.Genres);

            await context.SaveChangesAsync();

            var service = new TrackServices(context, mapper);

            var genre = new Genre { Id = int.MaxValue, Name = "Unexisting", IsDeleted = false };

            //Act && Assert
            await service.GetTracksByGenreAsync(genre);
        }

        [TestMethod]
        public async Task GetAllTracksAsync_Should_Return_AllTracks()
        {
            //Arrange
            await context.AddRangeAsync(Seed.Tracks);
            await context.AddRangeAsync(Seed.Genres);

            await context.SaveChangesAsync();

            var service = new TrackServices(context, mapper);

            //Act
            var actual = await service.GetAllTracksAsync();

            //Assert
            Assert.AreEqual(Seed.Tracks.Count(), actual.Count());
        }

        [TestMethod]
        public async Task GetTopXTracksAsync_Should_Return_TopXTracksOfGenre()
        {
            //Arrange
            await context.AddRangeAsync(Seed.Tracks);
            await context.AddRangeAsync(Seed.Genres);

            await context.SaveChangesAsync();

            var service = new TrackServices(context, mapper);

            var genre = Seed.Genres.FirstOrDefault(x => x.Name == "Rap");
            var top = 3;

            //Act
            var actual = await service.GetTopXTracksAsync(top, genre);
            var expected = Seed.Tracks.Where(track => track.GenreId == genre.Id)
                .OrderByDescending(x => x.Rank)
                .Take(top);

            //Assert
            Assert.AreEqual(expected.Count(), actual.Count());
            Assert.AreEqual(expected.First().Id, actual.First().Id);
        }

        [TestMethod]
        public async Task GetTopXTracksAsync_Should_Return_TopXTracks()
        {
            //Arrange
            await context.AddRangeAsync(Seed.Tracks);
            await context.AddRangeAsync(Seed.Genres);

            await context.SaveChangesAsync();

            var service = new TrackServices(context, mapper);

            var top = 3;

            //Act
            var actual = await service.GetTopXTracksAsync(top);
            var expected = Seed.Tracks.OrderByDescending(x => x.Rank)
                .Take(top);

            //Assert
            Assert.AreEqual(expected.Count(), actual.Count());
            Assert.AreEqual(expected.First().Id, actual.First().Id);
        }
    }
}
