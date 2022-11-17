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

        private IQueryable<Track> Get()
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
        
        //takes tracks from db shuffles it with orderby and leaves only the distinct artists
        public IEnumerable<Track> GetTracksWithDistinctArtists(Genre genre, int duration)
        {
            var tracks = this.Get().Where(x => x.GenreId == genre.Id)
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

        private bool IsDurationSatisfied(ref int duration, int songDuration)
        {
            duration -= songDuration;

            return -100 >= duration && duration >= -500;
        }

      /*  public IQueryable<TrackDTO> GetRandTracksByGenreAndDuration(Genre genre, int duration)
        {

        }*/
    }
}