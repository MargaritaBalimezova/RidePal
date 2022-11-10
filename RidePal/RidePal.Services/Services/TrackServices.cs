using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RidePal.Data;
using RidePal.Data.Models;
using RidePal.Services.DTOModels;
using RidePal.Services.Exceptions;
using RidePal.Services.Helpers;
using RidePal.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RidePal.Services.Services
{
    public class TrackServices : ITrackServices
    {
        private readonly RidePalContext db;
        private readonly IMapper mapper;

        public TrackServices(RidePalContext ridePalContext,IMapper mapper)
        {
            this.db = ridePalContext;
            this.mapper = mapper;
        }

        public IQueryable<TrackDTO> Get()
        {
            var tracks = this.db.Tracks.Select(x => this.mapper.Map<TrackDTO>(x)).AsQueryable();

            return tracks;
        }

        public TrackDTO GetByIdAsync(int id)
        {
            var track = this.Get().FirstOrDefault(x => x.Id == id)
                ?? throw new EntityNotFoundException(string.Format(Constants.TRACK_NOT_FOUND, id));

            return this.mapper.Map<TrackDTO>(track);
        }

        public async Task<TrackDTO> PostAsync(TrackDTO obj)
        {
            var artist = await this.db.Artists.FirstOrDefaultAsync(x => x.Id == obj.ArtistId)
                ?? throw new EntityNotFoundException(Constants.ARTIST_NOT_FOUND);

            var genre = await this.db.Genres.FirstOrDefaultAsync(x => x.Id == obj.GenreId)
                ?? throw new EntityNotFoundException(Constants.GENRE_NOT_FOUND);

            
            if(obj.Title == null)
            {
                throw new InvalidOperationException(Constants.INVALID_DATA);
            }

            var track = new Track
            {
                Title = obj.Title,
                Rank = (int)obj.Rank,
                Duration = obj.Duration,
                //need to check it
                //Album = mapper.Map<Album>(obj.Album),
                AlbumId = obj.AlbumId,
                Genre = genre,
                Artist = artist,
                PreviewURL = obj.PreviewURL,
                ImagePath = obj.ImagePath
            };

            await db.Tracks.AddAsync(track);
            await db.SaveChangesAsync();

            return this.mapper.Map<TrackDTO>(track);
        }

        public async Task<TrackDTO> UpdateAsync(int id, TrackDTO obj)
        {
            /*var track = await this.db.Tracks.FirstOrDefaultAsync(x => x.Id == id)
                ?? throw new EntityNotFoundException(Constants.TRACK_NOT_FOUND);

            if(obj.Title != null && obj.Title != track.Title)
            {
                track.Title = obj.Title;
            }
            if(obj.Rank != null && track.Rank != (int)obj.Rank)
            {
                track.Rank = (int)obj.Rank;
            }
            if(obj.Duration != track.Duration)
            {
                track.Duration = obj.Duration;
            }
            if(obj.Album != null)
            {
                track.Album = this.mapper.Map<Album>(obj.Album);
            }
            if()*/
            throw new NotImplementedException();
        }

        public Task<TrackDTO> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
