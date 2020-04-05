using AutoMapper;
using MahjongBuddy.Core;

namespace MahjongBuddy.Application.Games
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Game, GameDto>();
            CreateMap<UserGame, PlayerDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(s => s.AppUser.UserName))
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(s => s.AppUser.DisplayName));
            CreateMap<RoundTile, RoundTileDto>()
                .ForMember(dest => dest.TileImagePath, opt => opt.MapFrom(s => s.Tile.Image));
        }
    }
}
