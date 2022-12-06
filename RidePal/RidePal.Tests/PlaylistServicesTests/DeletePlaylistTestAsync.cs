using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MovieForum.Web.MappingConfig;
using RidePal.Data;
using RidePal.Services.Services;
using RidePal.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RidePal.Tests.PlaylistServicesTests
{
    [TestClass]
    public class DeletePlaylistTestAsync
    {
        private static IMapper mapper;
        private RidePalContext context;

        public DeletePlaylistTestAsync()
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
        public async Task DeleteAsync_Should_Delete_PlaylistByName()
        {
            //Arrange
            await context.AddRangeAsync(Seed.Playlists);

            await context.SaveChangesAsync();

            var genre = new GenreServices(context, mapper);
            var track = new TrackServices(context, mapper);
            var pixabay = new PixabayServices(new System.Net.Http.HttpClient());
            var aws = new AWSCloudStorageServices();
            var service = new PlaylistServices(context, mapper, genre, track, pixabay, aws);

            var title = "PlaylistN=3";

            //Act
            var actual = await service.DeleteAsync(title);

            //Assert
            Assert.IsFalse(context.Playlists.Any(x => x.Name == title));
        }
    }
}
