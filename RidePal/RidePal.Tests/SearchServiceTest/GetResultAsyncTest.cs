using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RidePal.Data;
using RidePal.Services.Services;
using RidePal.Tests.Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RidePal.Tests.SearchServiceTest
{
    [TestClass]
    public class GetResultAsyncTest
    {
        private static IMapper mapper;
        private RidePalContext context;

        public GetResultAsyncTest()
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
        public async Task SearchAlbumssAsync_Should_Return_AlbumsMatchingTheName()
        {
            //Arrange
            await context.AddRangeAsync(Seed.Albums);

            await context.SaveChangesAsync();

            var service = new SearchServices(context, mapper);

            var name = "her loss";

            //Act
            var actual = await service.SearchAlbumssAsync(name);
            var expected = Seed.Albums
                               .Where(album => album.Name.ToLower().Contains(name));

            //Assert
            Assert.AreEqual(expected.Count(), actual.Count());
        }

        [TestMethod]
        public async Task SearchArtistsAsync_Should_Return_ArtistMatchingTheName()
        {
            //Arrange
            await context.AddRangeAsync(Seed.Artists);

            await context.SaveChangesAsync();

            var service = new SearchServices(context, mapper);

            var name = "drake";

            //Act
            var actual = await service.SearchArtistsAsync(name);
            var expected = Seed.Artists
                               .Where(artist => artist.Name.ToLower().Contains(name));

            //Assert
            Assert.AreEqual(expected.Count(), actual.Count());
        }

        [TestMethod]
        public async Task SearchTracksAsync_Should_Return_TracksMatchingTheName()
        {
            //Arrange
            await context.AddRangeAsync(Seed.Tracks);
            await context.AddRangeAsync(Seed.Artists);

            await context.SaveChangesAsync();

            var service = new SearchServices(context, mapper);

            var name = "drake";
            var artist = Seed.Artists.FirstOrDefault(x => x.Name.ToLower() == name);

            //Act
            var actual = await service.SearchTracksAsync(name);
            var expected = Seed.Tracks
                               .Where(track => track.Title.ToLower().Contains(name) || track.ArtistId == artist.Id);

            //Assert
            Assert.AreEqual(expected.Count(), actual.Count());
        }

        [TestMethod]
        public async Task SearchUsersAsync_Should_Return_UsersMatchingTheName()
        {
            //Arrange
            await context.AddRangeAsync(Seed.Users);

            await context.SaveChangesAsync();

            var service = new SearchServices(context, mapper);

            var name = "angel";

            //Act
            var actual = await service.SearchUsersAsync(name);
            var expected = Seed.Users
                               .Where(user => user.Username.ToLower().Contains(name));

            //Assert
            Assert.AreEqual(expected.Count(), actual.Count());
        }

        [TestMethod]
        public async Task SearchPlaylistsAsync_Should_Return_PlaylistsMatchingTheName()
        {
            //Arrange
            await context.AddRangeAsync(Seed.Playlists);
            await context.AddRangeAsync(Seed.Audiences);

            await context.SaveChangesAsync();

            var service = new SearchServices(context, mapper);

            var name = "playlist";

            //Act
            var actual = await service.SearchPlaylistsAsync(name);
            var expected = Seed.Playlists
                               .Where(playlist => playlist.Name.ToLower().Contains(name) && playlist.AudienceId == 1);

            //Assert
            Assert.AreEqual(expected.Count(), actual.Count());
        }
    }
}
