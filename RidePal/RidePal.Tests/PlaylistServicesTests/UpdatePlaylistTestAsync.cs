using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RidePal.Data;
using RidePal.Services.DTOModels;
using RidePal.Services.Services;
using RidePal.Tests.Helpers;
using System;
using System.Threading.Tasks;

namespace RidePal.Tests.PlaylistServicesTests
{
    [TestClass]
    public class UpdatePlaylistTestAsync
    {
        private static IMapper mapper;
        private RidePalContext context;

        public UpdatePlaylistTestAsync()
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
        public async Task UpdateAsync_Should_Return_UpdatedVersionOfPlaylist()
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
            var id = 1;
            UpdatePlaylistDTO obj = new UpdatePlaylistDTO
            {
                Name = "NewName",
                Audience = Seed.Audiences[1]
            };
            //Act
            var actual = await service.UpdateAsync(id, obj);

            //Assert
            Assert.AreEqual(obj.Name, actual.Name);
            Assert.AreEqual(obj.Audience.Id, actual.Audience.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception),
            "UpdateAsync name validation failed!")]
        public async Task UpdateAsync_Should_ThrowException_When_NameIsEmpty()
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
            var id = 1;
            UpdatePlaylistDTO obj = new UpdatePlaylistDTO
            {
                Name = "",
                Audience = Seed.Audiences[1]
            };
            //Act && Assert
            await service.UpdateAsync(id, obj);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception),
           "UpdateAsync name validation failed!")]
        public async Task UpdateAsync_Should_ThrowException_When_NameIsExisting()
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
            var id = 1;
            UpdatePlaylistDTO obj = new UpdatePlaylistDTO
            {
                Name = "PlaylistN=3",
                Audience = Seed.Audiences[1]
            };
            //Act && Assert
            await service.UpdateAsync(id, obj);
        }
    }
}
