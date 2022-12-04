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

namespace RidePal.Tests.AlbumServiceTests
{
    [TestClass]
    public class GetAlbumsAsync
    {
        private static IMapper mapper;
        private RidePalContext context;

        public GetAlbumsAsync()
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
        public async Task GetAlbumById_Should_Return_Album()
        {
            //Arrange
            await context.AddRangeAsync(Seed.Albums);

            await context.SaveChangesAsync();

            var service = new AlbumServices(context, mapper);

            var id = 1;

            //Act
            var actual = await service.GetAlbumByIdAsync(id);
            var expected = mapper.Map<AlbumDTO>(Seed.Albums.FirstOrDefault(x => x.Id == id));

            //Assert
            Assert.AreEqual(expected.Id, actual.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(EntityNotFoundException), 
            "GetAlbumById_Should_ThrowException_When_IdIsInvalid failed to throw exception!")]
        public async Task GetAlbumById_Should_ThrowException_When_IdIsInvalid()
        {
            //Arrange
            await context.AddRangeAsync(Seed.Albums);

            await context.SaveChangesAsync();

            var service = new AlbumServices(context, mapper);

            var id = int.MaxValue;

            //Act && Assert
            await service.GetAlbumByIdAsync(id);
        }

        [TestMethod]
        public async Task GetSuggestedAlbums_Should_Return_SuggestedAlbumsByGenre()
        {
            //Arrange
            await context.AddRangeAsync(Seed.Albums);
            await context.AddRangeAsync(Seed.Tracks);

            await context.SaveChangesAsync();

            var service = new AlbumServices(context, mapper);

            var id = 1;
            var genreId = 1;

            //Act
            var actual = await service.GetSuggestedAlbums(genreId, id);
            var expected = mapper.Map<List<AlbumDTO>>(Seed.Albums
                .Where(x => x.GenreId == genreId && x.Id != id));

            //Assert
            Assert.AreEqual(expected.Count(), actual.Count());
        }

        [TestMethod]
        public async Task GetAlbumByGenreAsync_Should_Return_AlbumByGenre()
        {
            //Arrange
            await context.AddRangeAsync(Seed.Albums);

            await context.SaveChangesAsync();

            var service = new AlbumServices(context, mapper);

            var genre = mapper.Map<GenreDTO>(Seed.Genres.FirstOrDefault(x => x.Name == "Pop"));

            //Act
            var actual = await service.GetAlbumByGenreAsync(genre);
            var expected = mapper.Map<List<AlbumDTO>>(Seed.Albums
                .Where(x => x.GenreId == genre.Id));

            //Assert
            Assert.AreEqual(expected.Count(), actual.Count());
        }
    }
}
