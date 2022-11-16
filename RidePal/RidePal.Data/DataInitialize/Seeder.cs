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
            /*         if (System.Diagnostics.Debugger.IsAttached == false)
                     {
                         System.Diagnostics.Debugger.Launch();
                     }*/
            var fetchSongs = new FetchSongs();

            var result = await fetchSongs.GetTracksAsync("https://api.deezer.com/user/917475151/playlists", new Data.Models.Genre { Id = 1, Name = "Rap", IsDeleted = false });

            db.Entity<Genre>().HasData(new Data.Models.Genre { Id = 1, Name = "Rap", IsDeleted = false });
            db.Entity<Album>().HasData(result.albums);
            db.Entity<Artist>().HasData(result.artists);
            db.Entity<Track>().HasData(result.tracks);

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
                        RoleId = 2,
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
                        RoleId = 2,
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
        }
    }
}