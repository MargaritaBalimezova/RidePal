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
                .ReverseMap();

            this.CreateMap<Artist, ArtistDTO>()
                .ReverseMap();

            this.CreateMap<Genre, GenreDTO>()
                .ReverseMap();

            this.CreateMap<User, UserDTO>()
                .ForMember(dest => dest.Id, act => act.MapFrom(src => src.Id))
                .ForMember(dest => dest.RoleId, act => act.MapFrom(src => src.Role.Id))
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

            //this.CreateMap<MovieTags, MovieTagsDTO>()
            //    .ForMember(dest => dest.TagName, act => act.MapFrom(src => src.Tag.TagName))
            //    .ReverseMap();
        }
    }
}