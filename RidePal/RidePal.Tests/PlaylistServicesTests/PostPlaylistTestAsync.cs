using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RidePal.Data;
using RidePal.Services.DTOModels;
using RidePal.Services.Models;
using RidePal.Services.Services;
using RidePal.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RidePal.Tests.PlaylistServicesTests
{
    [TestClass]
    public class PostPlaylistTestAsync
    {
        private static IMapper mapper;
        private RidePalContext context;

        public PostPlaylistTestAsync()
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
        public async Task PostAsync_Should_Create_PlaylistWithRepeatArtist()
        {
            //Arrange
            await context.AddRangeAsync(Seed.Playlists);
            await context.AddRangeAsync(Seed.Tracks);
            await context.AddRangeAsync(Seed.Genres);

            await context.SaveChangesAsync();

            var genre = new GenreServices(context, mapper);
            var track = new TrackServices(context, mapper);
            var pixabay = new PixabayServices(new System.Net.Http.HttpClient());
            var aws = new AWSCloudStorageServices();
            var service = new PlaylistServices(context, mapper, genre, track, pixabay, aws);

            var playlist = new PlaylistDTO
            {
                Name = "NewlyCreatedPlaylist",
                ImagePath = "",
                Trip = new TripDTO
                {
                    Duration = 12
                },
                Audience = Seed.Audiences[0],
                RepeatArtists = true,
                TopSongs = false,
                GenresWithPercentages = new List<GenreWithPercentage>()
                {
                    new GenreWithPercentage
                    {
                        GenreName = "Rap",
                        Percentage = 90
                    },
                    new GenreWithPercentage
                    {
                        GenreName = "Pop",
                        Percentage = 10
                    }
                }
            };

            //Act
            var actual = await service.PostAsync(playlist);

            //Assert
            Assert.AreEqual(playlist.Name, actual.Name);
        }

        [TestMethod]
        public async Task PostAsync_Should_Create_PlaylistWithTopSongs()
        {
            //Arrange
            await context.AddRangeAsync(Seed.Playlists);
            await context.AddRangeAsync(Seed.Tracks);
            await context.AddRangeAsync(Seed.Genres);

            await context.SaveChangesAsync();

            var genre = new GenreServices(context, mapper);
            var track = new TrackServices(context, mapper);
            var pixabay = new PixabayServices(new System.Net.Http.HttpClient());
            var aws = new AWSCloudStorageServices();
            var service = new PlaylistServices(context, mapper, genre, track, pixabay, aws);

            var playlist = new PlaylistDTO
            {
                Name = "NewlyCreatedPlaylist",
                ImagePath = "",
                Trip = new TripDTO
                {
                    Duration = 12
                },
                Audience = Seed.Audiences[0],
                RepeatArtists = false,
                TopSongs = true,
                GenresWithPercentages = new List<GenreWithPercentage>()
                {
                    new GenreWithPercentage
                    {
                        GenreName = "Rap",
                        Percentage = 90
                    },
                    new GenreWithPercentage
                    {
                        GenreName = "Pop",
                        Percentage = 10
                    }
                }
            };

            //Act
            var actual = await service.PostAsync(playlist);

            //Assert
            Assert.AreEqual(playlist.Name, actual.Name);
        }

        [TestMethod]
        public async Task PostAsync_Should_Create_PlaylistWithTopSongsAndRepeatArtist()
        {
            //Arrange
            await context.AddRangeAsync(Seed.Playlists);
            await context.AddRangeAsync(Seed.Tracks);
            await context.AddRangeAsync(Seed.Genres);

            await context.SaveChangesAsync();

            var genre = new GenreServices(context, mapper);
            var track = new TrackServices(context, mapper);
            var pixabay = new PixabayServices(new System.Net.Http.HttpClient());
            var aws = new AWSCloudStorageServices();
            var service = new PlaylistServices(context, mapper, genre, track, pixabay, aws);

            var playlist = new PlaylistDTO
            {
                Name = "NewlyCreatedPlaylist",
                ImagePath = "",
                Trip = new TripDTO
                {
                    Duration = 12
                },
                Audience = Seed.Audiences[0],
                RepeatArtists = true,
                TopSongs = true,
                GenresWithPercentages = new List<GenreWithPercentage>()
                {
                    new GenreWithPercentage
                    {
                        GenreName = "Rap",
                        Percentage = 90
                    },
                    new GenreWithPercentage
                    {
                        GenreName = "Pop",
                        Percentage = 10
                    }
                }
            };

            //Act
            var actual = await service.PostAsync(playlist);

            //Assert
            Assert.AreEqual(playlist.Name, actual.Name);
        }

        [TestMethod]
        public async Task PostAsync_Should_Create_Playlist()
        {
            //Arrange
            await context.AddRangeAsync(Seed.Playlists);
            await context.AddRangeAsync(Seed.Tracks);
            await context.AddRangeAsync(Seed.Genres);

            await context.SaveChangesAsync();

            var genre = new GenreServices(context, mapper);
            var track = new TrackServices(context, mapper);
            var pixabay = new PixabayServices(new System.Net.Http.HttpClient());
            var aws = new AWSCloudStorageServices();
            var service = new PlaylistServices(context, mapper, genre, track, pixabay, aws);

            var playlist = new PlaylistDTO
            {
                Name = "NewlyCreatedPlaylist",
                ImagePath = "",
                Trip = new TripDTO
                {
                    Duration = 12
                },
                Audience = Seed.Audiences[0],
                RepeatArtists = false,
                TopSongs = false,
                GenresWithPercentages = new List<GenreWithPercentage>()
                {
                    new GenreWithPercentage
                    {
                        GenreName = "Rap",
                        Percentage = 90
                    },
                    new GenreWithPercentage
                    {
                        GenreName = "Pop",
                        Percentage = 10
                    }
                }
            };

            //Act
            var actual = await service.PostAsync(playlist);

            //Assert
            Assert.AreEqual(playlist.Name, actual.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task PostAsync_Should_ThrowException_When_PlaylistNameIsInvalid()
        {
            //Arrange
            await context.AddRangeAsync(Seed.Playlists);
            await context.AddRangeAsync(Seed.Tracks);
            await context.AddRangeAsync(Seed.Genres);

            await context.SaveChangesAsync();

            var genre = new GenreServices(context, mapper);
            var track = new TrackServices(context, mapper);
            var pixabay = new PixabayServices(new System.Net.Http.HttpClient());
            var aws = new AWSCloudStorageServices();
            var service = new PlaylistServices(context, mapper, genre, track, pixabay, aws);

            var playlist = new PlaylistDTO
            {
                Name = "Ne",
                ImagePath = "",
                Trip = new TripDTO
                {
                    Duration = 12
                },
                Audience = Seed.Audiences[0],
                RepeatArtists = false,
                TopSongs = false,
                GenresWithPercentages = new List<GenreWithPercentage>()
                {
                    new GenreWithPercentage
                    {
                        GenreName = "Rap",
                        Percentage = 90
                    },
                    new GenreWithPercentage
                    {
                        GenreName = "Pop",
                        Percentage = 10
                    }
                }
            };

            //Act and Assert
            await service.PostAsync(playlist);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task PostAsync_Should_ThrowException_When_PlaylistNameIsExisting()
        {
            //Arrange
            await context.AddRangeAsync(Seed.Playlists);
            await context.AddRangeAsync(Seed.Tracks);
            await context.AddRangeAsync(Seed.Genres);

            await context.SaveChangesAsync();

            var genre = new GenreServices(context, mapper);
            var track = new TrackServices(context, mapper);
            var pixabay = new PixabayServices(new System.Net.Http.HttpClient());
            var aws = new AWSCloudStorageServices();
            var service = new PlaylistServices(context, mapper, genre, track, pixabay, aws);

            var playlist = new PlaylistDTO
            {
                Name = "PlaylistN=1",
                ImagePath = "",
                Trip = new TripDTO
                {
                    Duration = 12
                },
                Audience = Seed.Audiences[0],
                RepeatArtists = false,
                TopSongs = false,
                GenresWithPercentages = new List<GenreWithPercentage>()
                {
                    new GenreWithPercentage
                    {
                        GenreName = "Rap",
                        Percentage = 90
                    },
                    new GenreWithPercentage
                    {
                        GenreName = "Pop",
                        Percentage = 10
                    }
                }
            };

            //Act and Assert
            await service.PostAsync(playlist);
        }
    }
}
