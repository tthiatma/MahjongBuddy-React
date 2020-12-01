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
            CreateMap<Connection, ConnectionDto>();
            CreateMap<Game, GameDto>()
                .ForMember(dest => dest.HostUserName, opt => opt.MapFrom(s => s.Host.UserName));
            CreateMap<GamePlayer, PlayerDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(s => s.Player.UserName))
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(s => s.Player.DisplayName))
                .ForMember(dest => dest.Image, opt => opt.MapFrom(s => s.Player.Photos.FirstOrDefault(x => x.IsMain).Url))
                .ForMember(dest => dest.Connections, opt => opt.MapFrom(s => s.Connections));        
        }
    }
}