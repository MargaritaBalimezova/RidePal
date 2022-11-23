﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RidePal.Data;
using RidePal.Services.DTOModels;
using RidePal.Services.Exceptions;
using RidePal.Services.Helpers;
using RidePal.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RidePal.Services.Services
{
    public class GenreServices : IGenreService
    {
        private readonly RidePalContext db;
        private readonly IMapper mapper;

        public GenreServices(RidePalContext ridePalContext, IMapper mapper)
        {
            this.db = ridePalContext;
            this.mapper = mapper;
        }

        public async Task<GenreDTO> GetGenreById(int id)
        {
            var genre = await this.db.Genres
                .FirstOrDefaultAsync(x => x.Id == id)
                ?? throw new EntityNotFoundException(Constants.GENRE_NOT_FOUND);

            return this.mapper.Map<GenreDTO>(genre);
        }

        public async Task<GenreDTO> GetGenreByName(string GenreName)
        {
            var genre = await this.db.Genres
                .FirstOrDefaultAsync(x => x.Name.ToLower() == GenreName.ToLower())
                ?? throw new EntityNotFoundException(Constants.GENRE_NOT_FOUND);

            return this.mapper.Map<GenreDTO>(genre);
        }
    }
}
