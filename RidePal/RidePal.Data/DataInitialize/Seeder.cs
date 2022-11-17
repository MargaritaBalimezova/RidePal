using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RidePal.Data.DataInitialize.Interfaces;
using RidePal.Data.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RidePal.Data.DataInitialize
{
    public static class Seeder
    {
        public static async Task Seed(this ModelBuilder db)
        {
            //TODO: remove it
            /*            if (System.Diagnostics.Debugger.IsAttached == false)
                        {
                            System.Diagnostics.Debugger.Launch();
                        }*/

            var roles = new List<Role>()
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

            db.Entity<Role>().HasData(roles);

            var users = new List<User>()
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
                        ImagePath = "default.jpg",
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
                        ImagePath = "default.jpg",
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
                        ImagePath = "default.jpg",
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
                        ImagePath = "default.jpg",
                        IsEmailConfirmed = true,
                        IsGoogleAccount = false
                    }
            };

            var passwordHasher = new PasswordHasher<User>();
            foreach (var item in users)
            {
                item.Password = passwordHasher.HashPassword(item, item.Password);
            }

            db.Entity<User>().HasData(users);

            List<Genre> genres = new List<Genre>
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

            db.Entity<Genre>().HasData(genres);

            List<Album> albums = new List<Album>();
            List<Artist> artists = new List<Artist>();
            List<Track> tracks = new List<Track>();

            var fetchSongs = new FetchSongs();

            //fecth Rap songs
            var result = await fetchSongs.GetTracksAsync("https://api.deezer.com/user/917475151/playlists", genres[0]);

            albums.AddRange(result.albums);
            artists.AddRange(result.artists);
            tracks.AddRange(result.tracks);

            //fecth Rock songs
            await Task.Delay(5000);
            result = await fetchSongs.GetTracksAsync("https://api.deezer.com/user/753566875/playlists", genres[1]);

            albums.AddRange(result.albums);
            artists.AddRange(result.artists);
            tracks.AddRange(result.tracks);

            //fecth Pop songs
            await Task.Delay(5000);
            result = await fetchSongs.GetTracksAsync("https://api.deezer.com/user/5179240022/playlists", genres[2]);

            albums.AddRange(result.albums);
            artists.AddRange(result.artists);
            tracks.AddRange(result.tracks);

            //fecth Dance & EDM songs
            await Task.Delay(5000);
            result = await fetchSongs.GetTracksAsync("https://api.deezer.com/user/2834392844/playlists", genres[3]);

            albums.AddRange(result.albums);
            artists.AddRange(result.artists);
            tracks.AddRange(result.tracks);

            //fecth Latin songs
            await Task.Delay(5000);
            result = await fetchSongs.GetTracksAsync("https://api.deezer.com/user/3115986664/playlists", genres[4]);

            albums.AddRange(result.albums);
            artists.AddRange(result.artists);
            tracks.AddRange(result.tracks);

            //fecth Classical songs
            await Task.Delay(5000);
            result = await fetchSongs.GetTracksAsync("https://api.deezer.com/user/353978015/playlists", genres[5]);

            albums.AddRange(result.albums);
            artists.AddRange(result.artists);
            tracks.AddRange(result.tracks);

            //fecth CHALGA songs
            //await Task.Delay(5000);
            //result = await fetchSongs.GetTracksAsync("https://api.deezer.com/user/5174896922/playlists", genres[6]);

            //albums.AddRange(result.albums);
            //artists.AddRange(result.artists);
            //tracks.AddRange(result.tracks);

            db.Entity<Album>().HasData(result.albums);
            db.Entity<Artist>().HasData(result.artists);
            db.Entity<Track>().HasData(result.tracks);
        }
    }
}