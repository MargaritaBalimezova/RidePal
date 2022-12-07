using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RidePal.Data;
using RidePal.Services.Models;
using RidePal.Services.Services;
using RidePal.Tests.Helpers;
using RidePal.Web.MappingConfig;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RidePal.Tests.PlaylistServicesTests
{
    [TestClass]
    public class GetPlaylistTestAsync
    {
        private static IMapper mapper;
        private RidePalContext context;

        public GetPlaylistTestAsync()
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
        public async Task GetAsync_Should_Return_AllPlaylists()
        {
            //Arrange
            await context.AddRangeAsync(Seed.Playlists);

            await context.SaveChangesAsync();

            var genre = new GenreServices(context, mapper);
            var track = new TrackServices(context, mapper);
            var pixabay = new PixabayServices(new System.Net.Http.HttpClient());
            var aws = new AWSCloudStorageServices();
            var service = new PlaylistServices(context, mapper, genre, track, pixabay, aws);

            //Act
            var actual = await service.GetAsync();

            //Assert
            Assert.AreEqual(Seed.Playlists.Count(), actual.Count());
        }

        [TestMethod]
        public async Task GetPlaylistDTOAsync_Should_Return_PlaylistsWithName()
        {
            //Arrange
            await context.AddRangeAsync(Seed.Playlists);

            await context.SaveChangesAsync();

            var genre = new GenreServices(context, mapper);
            var track = new TrackServices(context, mapper);
            var pixabay = new PixabayServices(new System.Net.Http.HttpClient());
            var aws = new AWSCloudStorageServices();
            var service = new PlaylistServices(context, mapper, genre, track, pixabay, aws);

            var name = "PlaylistN=1";
            //Act
            var actual = await service.GetPlaylistDTOAsync(name);

            //Assert
            Assert.AreEqual(name, actual.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task GetPlaylistDTOAsync_Should_ThrowException_When_PlaylistsNameInvalid()
        {
            //Arrange
            await context.AddRangeAsync(Seed.Playlists);

            await context.SaveChangesAsync();

            var genre = new GenreServices(context, mapper);
            var track = new TrackServices(context, mapper);
            var pixabay = new PixabayServices(new System.Net.Http.HttpClient());
            var aws = new AWSCloudStorageServices();
            var service = new PlaylistServices(context, mapper, genre, track, pixabay, aws);

            var name = "invalid=1";
            //Act and Assert
            await service.GetPlaylistDTOAsync(name);
        }

        [TestMethod]
        public async Task GetPlaylistDTOAsync_Should_Return_PlaylistsWithId()
        {
            //Arrange
            await context.AddRangeAsync(Seed.Playlists);

            await context.SaveChangesAsync();

            var genre = new GenreServices(context, mapper);
            var track = new TrackServices(context, mapper);
            var pixabay = new PixabayServices(new System.Net.Http.HttpClient());
            var aws = new AWSCloudStorageServices();
            var service = new PlaylistServices(context, mapper, genre, track, pixabay, aws);

            var id = 1;
            //Act
            var actual = await service.GetPlaylistDTOAsync(id);

            //Assert
            Assert.AreEqual(id, actual.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task GetPlaylistDTOAsync_Should_ThrowException_When_PlaylistsIdInvalid()
        {
            //Arrange
            await context.AddRangeAsync(Seed.Playlists);

            await context.SaveChangesAsync();

            var genre = new GenreServices(context, mapper);
            var track = new TrackServices(context, mapper);
            var pixabay = new PixabayServices(new System.Net.Http.HttpClient());
            var aws = new AWSCloudStorageServices();
            var service = new PlaylistServices(context, mapper, genre, track, pixabay, aws);

            var id = int.MaxValue;
            //Act and Assert
            await service.GetPlaylistDTOAsync(id);
        }

        [TestMethod]
        public async Task GetUserPlaylists_Should_Return_PlaylistsWithUserId()
        {
            //Arrange
            await context.AddRangeAsync(Seed.Playlists);
            await context.AddRangeAsync(Seed.Users);

            await context.SaveChangesAsync();

            var genre = new GenreServices(context, mapper);
            var track = new TrackServices(context, mapper);
            var pixabay = new PixabayServices(new System.Net.Http.HttpClient());
            var aws = new AWSCloudStorageServices();
            var service = new PlaylistServices(context, mapper, genre, track, pixabay, aws);

            var id = 1;
            //Act
            var actual = await service.GetUserPlaylists(id);

            //Assert
            Assert.IsFalse(actual.Any(x => x.Id != id));
        }

        [TestMethod]
        public async Task GetAudienceAsync_Should_Return_Audience()
        {
            //Arrange
            await context.AddRangeAsync(Seed.Audiences);

            await context.SaveChangesAsync();

            var genre = new GenreServices(context, mapper);
            var track = new TrackServices(context, mapper);
            var pixabay = new PixabayServices(new System.Net.Http.HttpClient());
            var aws = new AWSCloudStorageServices();
            var service = new PlaylistServices(context, mapper, genre, track, pixabay, aws);

            var id = 1;
            //Act
            var actual = await service.GetAudienceAsync(id);

            //Assert
            Assert.AreEqual(Helpers.Seed.Audiences.FirstOrDefault(x => x.Id == id).Name
                , actual.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task GetAudienceAsync_Should_ThrowException_When_AudienceIdInvalid()
        {
            //Arrange
            await context.AddRangeAsync(Seed.Audiences);

            await context.SaveChangesAsync();

            var genre = new GenreServices(context, mapper);
            var track = new TrackServices(context, mapper);
            var pixabay = new PixabayServices(new System.Net.Http.HttpClient());
            var aws = new AWSCloudStorageServices();
            var service = new PlaylistServices(context, mapper, genre, track, pixabay, aws);

            var id = int.MaxValue;
            //Act and Assert
            await service.GetAudienceAsync(id);
        }

        [TestMethod]
        public async Task PlaylistCount_Should_Return_PlaylistsCount()
        {
            //Arrange
            await context.AddRangeAsync(Seed.Playlists);

            await context.SaveChangesAsync();

            var genre = new GenreServices(context, mapper);
            var track = new TrackServices(context, mapper);
            var pixabay = new PixabayServices(new System.Net.Http.HttpClient());
            var aws = new AWSCloudStorageServices();
            var service = new PlaylistServices(context, mapper, genre, track, pixabay, aws);

            //Act
            var actual = await service.PlaylistCount();

            //Assert
            Assert.AreEqual(Helpers.Seed.Playlists.Count(), actual);
        }

        [TestMethod]
        public async Task FilterPlaylists_Should_Return_FilteredPlaylistByDurationAndSortedByDuration()
        {
            //Arrange
            await context.AddRangeAsync(Seed.Playlists);
            await context.AddRangeAsync(Seed.Audiences);

            await context.SaveChangesAsync();

            var genre = new GenreServices(context, mapper);
            var track = new TrackServices(context, mapper);
            var pixabay = new PixabayServices(new System.Net.Http.HttpClient());
            var aws = new AWSCloudStorageServices();
            var service = new PlaylistServices(context, mapper, genre, track, pixabay, aws);

            var parameters = new PlaylistQueryParameters
            {
                Duration = 15,
                SortBy = "duration",
                SortOrder = "descending",
            };

            //Act
            var actual = await service.FilterPlaylists(parameters);

            //Assert
            Assert.IsTrue(actual.First().Duration == Seed.Playlists.Max(x => x.Duration));
        }

        [TestMethod]
        public async Task FilterPlaylists_Should_Return_FilteredPlaylistByDurationAndSortedByName()
        {
            //Arrange
            await context.AddRangeAsync(Seed.Playlists);
            await context.AddRangeAsync(Seed.Audiences);

            await context.SaveChangesAsync();

            var genre = new GenreServices(context, mapper);
            var track = new TrackServices(context, mapper);
            var pixabay = new PixabayServices(new System.Net.Http.HttpClient());
            var aws = new AWSCloudStorageServices();
            var service = new PlaylistServices(context, mapper, genre, track, pixabay, aws);

            var parameters = new PlaylistQueryParameters
            {
                Duration = 15,
                SortBy = "name",
                SortOrder = "descending",
            };

            //Act
            var actual = await service.FilterPlaylists(parameters);

            //Assert
            Assert.IsTrue(actual.First().Id == Seed.Playlists.OrderByDescending(x => x.Name).First().Id);
        }
    }
}