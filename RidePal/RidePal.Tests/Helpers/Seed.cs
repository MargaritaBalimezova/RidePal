using RidePal.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RidePal.Tests.Helpers
{
    public class Seed
    {
        public static List<Role> Role
        {
            get
            {
                return new List<Role>()
                {
                    new Role
                    {
                        Id = 1,
                        Name = "Admin"
                    },
                    new Role
                    {
                        Id = 2,
                        Name = "User"
                    }
                };
            }
        }

        public static List<User> Users
        {
            get
            {
                return new List<User>()
            {
                 new User
                    {
                        Id = 1,
                        Username = "AngelMarinski",
                        FirstName = "Angel",
                        LastName = "Marinski",
                        Password = "12345678",
                        Email = "fakeemail@gmail.com",
                        RoleId = 2,
                        IsDeleted = false,
                        ImagePath = "https://ridepalbucket.s3.amazonaws.com/default.jpg",
                        IsEmailConfirmed = true,
                        IsGoogleAccount = false
                    },
                 new User
                    {
                        Id = 2,
                        Username = "Maggie",
                        FirstName = "Maggie",
                        LastName = "TheBoss",
                        Password = "12345678",
                        Email = "adminsemail@gmail.com",
                        RoleId = 1,
                        IsDeleted = false,
                        ImagePath = "https://ridepalbucket.s3.amazonaws.com/default.jpg",
                        IsEmailConfirmed = true,
                        IsGoogleAccount = false
                    },
                  new User
                    {
                        Id = 3,
                        Username = "Rado561",
                        FirstName = "Radoslav",
                        LastName = "Berov",
                        Password = "12345678",
                        Email = "morefakeemails@gmail.com",
                        RoleId = 1,
                        IsDeleted = false,
                        ImagePath = "https://ridepalbucket.s3.amazonaws.com/default.jpg",
                        IsEmailConfirmed = true,
                        IsGoogleAccount = false
                    },
                   new User
                    {
                        Id = 4,
                        Username = "James96",
                        FirstName = "James",
                        LastName = "Bond",
                        Password = "12345678",
                        Email = "agent007@gmail.com",
                        RoleId = 1,
                        IsDeleted = false,
                        ImagePath = "https://ridepalbucket.s3.amazonaws.com/default.jpg",
                        IsEmailConfirmed = true,
                        IsGoogleAccount = false
                    }
                };
            }
        }

        public static List<Genre> Genres
        { 
            get
            {
                return new List<Genre>()
                {
                    new Genre
                    {
                        Id = 1,
                        Name = "Rap",
                        IsDeleted = false
                    },
                    new Genre
                    {
                        Id = 2,
                        Name = "Rock",
                        IsDeleted = false
                    },
                    new Genre
                    {
                        Id = 3,
                        Name = "Pop",
                        IsDeleted = false
                    },
                    new Genre
                    {
                        Id = 4,
                        Name = "Dance & EDM",
                        IsDeleted = false
                    },
                    new Genre
                    {
                        Id = 5,
                        Name = "Latin",
                        IsDeleted = false
                    },
                    new Genre
                    {
                        Id = 6,
                        Name = "Classical",
                        IsDeleted = false
                    },
                    new Genre
                    {
                        Id = 7,
                        Name = "Pop-Folk",
                        IsDeleted = false
                    }
                };
            } 
        }

        public static List<Artist> Artists
        {
            get
            {
                return new List<Artist>()
                {
                    new Artist
                    {
                        Id = 1,
                        Name = "Drake",
                        IsDeleted = false
                    },
                    new Artist
                    {
                        Id = 2,
                        Name = "21 Savage",
                        IsDeleted = false
                    },
                    new Artist
                    {
                        Id = 3,
                        Name = "Future",
                        IsDeleted = false
                    },
                    new Artist
                    {
                        Id = 4,
                        Name = "The Weeknd",
                        IsDeleted = false
                    },
                };
            }
        }

        public static List<Album> Albums
        {
            get
            {
                return new List<Album>()
                {
                    new Album
                    {
                        Id = 1,
                        Name = "Her Loss",
                        ArtistId = 1,
                        GenreId = 1,
                        IsDeleted = false
                    },
                    new Album
                    {
                        Id = 2,
                        Name = "I am > I was",
                        ArtistId = 2,
                        GenreId = 1,
                        IsDeleted = false
                    },
                    new Album
                    {
                        Id = 3,
                        Name = "High Off Life",
                        ArtistId = 3,
                        GenreId = 1,
                        IsDeleted = false
                    },
                    new Album
                    {
                        Id = 4,
                        Name = "Thursday",
                        ArtistId = 4,
                        GenreId = 3,
                        IsDeleted = false
                    },
                };
            }
        }

        public static List<Track> Tracks
        {
            get
            {
                return new List<Track>()
                {
                    new Track
                    {
                        Id = 1,
                        Title = "Rich Flex",
                        Rank = 10,
                        Duration = 500,
                        AlbumId = 1,
                        ArtistId = 1,
                        GenreId = 1,
                        PreviewURL = "",
                        ImagePath = "",
                        IsDeleted = false
                    },
                    new Track
                    {
                        Id = 2,
                        Title = "Spin Bout You",
                        Rank = 10,
                        Duration = 500,
                        AlbumId = 1,
                        ArtistId = 1,
                        GenreId = 1,
                        PreviewURL = "",
                        ImagePath = "",
                        IsDeleted = false
                    },
                    new Track
                    {
                        Id = 3,
                        Title = "a lot",
                        Rank = 7,
                        Duration = 500,
                        AlbumId = 2,
                        ArtistId = 2,
                        GenreId = 1,
                        PreviewURL = "",
                        ImagePath = "",
                        IsDeleted = false
                    },
                    new Track
                    {
                        Id = 4,
                        Title = "Too comfortable",
                        Rank = 9,
                        Duration = 500,
                        AlbumId = 3,
                        ArtistId = 3,
                        GenreId = 1,
                        PreviewURL = "",
                        ImagePath = "",
                        IsDeleted = false
                    },
                    new Track
                    {
                        Id = 5,
                        Title = "High For This",
                        Rank = 9,
                        Duration = 500,
                        AlbumId = 4,
                        ArtistId = 4,
                        GenreId = 3,
                        PreviewURL = "",
                        ImagePath = "",
                        IsDeleted = false
                    },
                };
            }
        }
    }
}
