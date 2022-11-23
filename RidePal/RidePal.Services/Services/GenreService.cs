using Microsoft.EntityFrameworkCore;
using RidePal.Data;
using RidePal.Data.Models;
using RidePal.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RidePal.Services.Services
{
    public class GenreService : IGenreService
    {
        private readonly RidePalContext db;

        public GenreService(RidePalContext db)
        {
            this.db = db;
        }

        public async Task<Genre> GetGenre(string name)
        {
            var genre = await db.Genres.FirstOrDefaultAsync(x => x.Name == name);
            return genre;
        }
    }
}