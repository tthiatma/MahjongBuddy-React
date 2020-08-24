using AutoMapper;
using MahjongBuddy.Application.Dtos;
using MahjongBuddy.Core;
using System.Linq;

namespace MahjongBuddy.Application.Games
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Game, GameDto>()
                .ForMember(dest => dest.HostUserName, opt => opt.MapFrom(s => s.Host.UserName));
            CreateMap<UserGame, PlayerDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(s => s.AppUser.UserName))
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(s => s.AppUser.DisplayName))
                .ForMember(dest => dest.Image, opt => opt.MapFrom(s => s.AppUser.Photos.FirstOrDefault(x => x.IsMain).Url));
        }
    }
}