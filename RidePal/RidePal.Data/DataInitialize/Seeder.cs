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
        public async static Task Seed(this ModelBuilder db)
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
        }
    }
}
