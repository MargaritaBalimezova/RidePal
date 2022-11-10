﻿using AutoMapper;
using RidePal.Data.Models;
using RidePal.Services.DTOModels;

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
                .ForMember(dest => dest.Role, act => act.MapFrom(src => src.Role.Name))
                .ReverseMap();

            this.CreateMap<User, UpdateUserDTO>()
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
