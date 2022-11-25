using AutoMapper;
using RidePal.Data.Models;
using RidePal.Services.DTOModels;
using RidePal.WEB.Models;

namespace MovieForum.Web.MappingConfig
{
    public class RidePalProfile : Profile
    {
        public RidePalProfile()
        {
            this.CreateMap<Track, TrackDTO>()
                .ReverseMap();

            this.CreateMap<Album, AlbumDTO>()
                .ForMember(dest => dest.ArtistName, act => act.MapFrom(src => src.Artist.Name))
                .ReverseMap();

            this.CreateMap<Artist, ArtistDTO>()
                .ReverseMap();

            this.CreateMap<Genre, GenreDTO>()
                .ReverseMap();

            this.CreateMap<User, UserDTO>()
                .ForMember(dest => dest.Id, act => act.MapFrom(src => src.Id))
                .ForMember(dest => dest.RoleId, act => act.MapFrom(src => src.Role.Id))
                .ForMember(dest => dest.RoleName, act => act.MapFrom(src => src.Role.Name))
                .ReverseMap();

            this.CreateMap<User, UpdateUserDTO>()
                .ReverseMap();

            this.CreateMap<User, LoginUserDTO>()
                .ReverseMap();

            this.CreateMap<UserDTO, LoginUserDTO>()
                .ReverseMap();

            this.CreateMap<UserDTO, UserViewModel>()
               .ReverseMap();

            this.CreateMap<UserDTO, UpdateUserDTO>()
                .ReverseMap();

            this.CreateMap<FriendRequest, FriendRequestDTO>()
                .ReverseMap();

            this.CreateMap<Playlist, PlaylistDTO>()
               .ReverseMap();

            this.CreateMap<PlaylistDTO, CreatePlaylistViewModel>()
               .ForMember(dest => dest.AudienceId, act => act.MapFrom(src => src.Audience.Id))
               .ReverseMap();

            this.CreateMap<PlaylistDTO, PlaylistViewModel>()
               .ReverseMap();

            this.CreateMap<UpdatePlaylistDTO, UpdatePlaylistViewModel>()
               .ForMember(dest => dest.AudienceId, act => act.MapFrom(src => src.Audience.Id))
               .ReverseMap();

            this.CreateMap<Track, TrackDTO>()
               .ReverseMap();

            this.CreateMap<PlaylistGenre, PlaylistGenreDTO>()
                .ForMember(dest => dest.Name, act => act.MapFrom(src => src.Genre.Name))
                .ReverseMap();

            this.CreateMap<PlaylistTracks, PlaylistTracksDTO>()
                .ReverseMap();

            this.CreateMap<TrackDTO, PlaylistTracksDTO>()
                .ForMember(dest => dest.Title, act => act.MapFrom(src => src.Title))
                .ForMember(dest => dest.TrackId, act => act.MapFrom(src => src.Id))
                .ReverseMap();

            this.CreateMap<Trip, TripDTO>()
                .ReverseMap();
        }
    }
}