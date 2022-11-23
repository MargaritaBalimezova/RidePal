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
using static RidePal.Services.Models.SpotifyResultModel;

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

        public TrackDTO GetByIdAsync(int id)
        {
            var track = this.Get().FirstOrDefault(x => x.Id == id)
                ?? throw new EntityNotFoundException(string.Format(Constants.TRACK_NOT_FOUND, id));

            return this.mapper.Map<TrackDTO>(track);
        }

        #region Maggie

        public IEnumerable<Track> GetTracksWithDistinctArtistsForPlaylist(Genre genre, double duration)
        {
            var tracksReady = this.Get()
                            .Where(x => x.GenreId == genre.Id)
                            .OrderBy(x => Guid.NewGuid())
                            .ToList()
                            .GroupBy(x => x.ArtistId)
                            .Select(x => x.First());

            var finalTracksReady = new List<Track>();

            var currentDuration = 0;

            foreach (var track in tracksReady)
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

        public IEnumerable<Track> GetTracksForPlaylist(Genre genre, double duration)
        {
            var tracksReady = this.Get().Where(x => x.GenreId == genre.Id)
                           .OrderBy(x => Guid.NewGuid())
                           .ToList();

            var finalTracksReady = new List<Track>();

            var currentDuration = 0;

            foreach (var track in tracksReady)
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

        #endregion Maggie

        //takes tracks from db shuffles it with orderby and leaves only the distinct artists
        public IEnumerable<Track> GetTracksWithDistinctArtists(Genre genre, int duration)
        {
            var tracks = this.Get()
                            .Where(x => x.GenreId == genre.Id)
                            .OrderBy(x => Guid.NewGuid())
                            .ToList()
                            .GroupBy(x => x.ArtistId)
                            .Select(x => x.First())
                            .TakeWhile(x => !IsDurationSatisfied(ref duration, x.Duration));

            return tracks;
        }

        public IEnumerable<Track> GetTracks(Genre genre, int duration)
        {
            var tracks = this.Get().Where(x => x.GenreId == genre.Id)
            .OrderBy(x => Guid.NewGuid())
            .ToList()
            .TakeWhile(x => !IsDurationSatisfied(ref duration, x.Duration));

            return tracks;
        }

        public async Task<IEnumerable<Track>> GetTracksByGenre(Genre genre)
        {
            var tracks = await this.Get()
                                .Where(x => x.GenreId == genre.Id)
                                .ToListAsync();

            if (tracks.Count == 0)
            {
                throw new InvalidOperationException(Constants.GENRE_NOT_FOUND);
            }

            return tracks;
        }

        public async Task<IEnumerable<Track>> GetTracksByGenreName(Genre genre)
        {
            var tracks = await this.Get()
                                .Where(x => x.Genre.Name == genre.Name)
                                .ToListAsync();

            if (tracks.Count == 0)
            {
                throw new InvalidOperationException(Constants.GENRE_NOT_FOUND);
            }

            return tracks;
        }

        public IEnumerable<Track> GetTracksSortedByRankDesc()
        {
            var tracks = this.Get()
                .Take(100).OrderByDescending(x => x.Rank);

            return tracks.ToList();
        }

        private bool IsDurationSatisfied(ref int duration, int songDuration)
        {
            duration -= songDuration;

            return -100 >= duration && duration >= -500;
        }
    }
}