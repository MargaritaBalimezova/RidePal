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

        public TrackServices(RidePalContext ridePalContext, IMapper mapper)
        {
            this.db = ridePalContext;
            this.mapper = mapper;
        }

        public IQueryable<Track> Get()
        {
            var tracks = this.db.Tracks.AsQueryable();

            return tracks;
        }
        public async Task<TrackDTO> GetByIdAsync(int id)
        {
            var track = await this.Get().FirstOrDefaultAsync(x => x.Id == id)
                ?? throw new EntityNotFoundException(string.Format(Constants.TRACK_NOT_FOUND, id));

            return this.mapper.Map<TrackDTO>(track);
        }
        
        //takes tracks from db shuffles it with orderby and leaves only the distinct artists
        public IEnumerable<TrackDTO> GetTracksWithDistinctArtists(Genre genre, int duration)
        {
            var tracks = this.Get()
                            .Where(x => x.GenreId == genre.Id)
                            .OrderBy(x => Guid.NewGuid())
                            .ToList()
                            .GroupBy(x => x.ArtistId)
                            .Select(x => x.First())
                            .ToList();

            tracks = GetFinalTracksForDurationX(tracks, duration);

            return this.mapper.Map<IEnumerable<TrackDTO>>(tracks);
        }

        public IEnumerable<TrackDTO> GetTracks(Genre genre, int duration)
        {
            var tracks = this.Get()
                            .Where(x => x.GenreId == genre.Id)
                            .OrderBy(x => Guid.NewGuid())
                            .ToList();

            tracks = GetFinalTracksForDurationX(tracks, duration);
            
            return this.mapper.Map<IEnumerable<TrackDTO>>(tracks);
        }

        public async Task<IEnumerable<TrackDTO>> GetTracksByGenreAsync(Genre genre)
        {
            var tracks = await this.Get()
                                .Where(x => x.GenreId == genre.Id)
                                .ToListAsync();

            if(tracks.Count == 0)
            {
                throw new InvalidOperationException(Constants.GENRE_NOT_FOUND);
            }

            return this.mapper.Map<IEnumerable<TrackDTO>>(tracks);
        }

        public async Task<IEnumerable<TrackDTO>> GetAllTracksAsync()
        {
            var tracks = await this.Get()
                .ToListAsync();

            return this.mapper.Map<IEnumerable<TrackDTO>>(tracks);
        }

        public async Task<IEnumerable<TrackDTO>> GetTopXTracksAsync(int x, Genre genre = null)
        {
            if (genre == null)
            {
                var tracks = await this.Get()
                .OrderBy(x => x.Rank)
                .Take(x)
                .ToListAsync();

                return this.mapper.Map<IEnumerable<TrackDTO>>(tracks);
            }

            var tracksByGenre = await this.Get()
                .Where(x => x.Genre.Id == genre.Id)
                .OrderBy(x => x.Rank)
                .Take(x)
                .ToListAsync();

            return this.mapper.Map<IEnumerable<TrackDTO>>(tracksByGenre);
        }

        private List<Track> GetFinalTracksForDurationX(List<Track> tracks, int duration)
        {
            var finalTracksReady = new List<Track>();

            var currentDuration = 0;

            foreach (var track in tracks)
            {
                if (!(duration - 60 <= currentDuration && currentDuration <= duration + 60))
                {
                    if (track.Duration <= duration - currentDuration + 60)
                    {
                        currentDuration += track.Duration;
                        finalTracksReady.Add(track);
                    }
                }
                else
                {
                    break;
                }
            }

            return finalTracksReady;
        }
    }
}