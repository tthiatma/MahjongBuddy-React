using AutoMapper;
using MahjongBuddy.Application.Dtos;
using MahjongBuddy.Application.Rounds.AutoMapperResolvers;
using MahjongBuddy.Core;
using System.Linq;

namespace MahjongBuddy.Application.Rounds
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<Round, RoundDto>()
                .ForMember(dest => dest.MainPlayer, opt => opt.MapFrom<MainPlayerResolver>())
                .ForMember(dest => dest.OtherPlayers, opt => opt.MapFrom<OtherPlayersResolver>());

            CreateMap<RoundTile, RoundTileDto>();
            CreateMap<RoundResultHand, RoundResultHandDto>();
            CreateMap<RoundResultExtraPoint, RoundResultExtraPointDto>();
            CreateMap<RoundPlayerAction, RoundPlayerActionDto>();

            CreateMap<RoundPlayer, RoundPlayerDto>()
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(s => s.AppUser.DisplayName))
                .ForMember(dest => dest.PlayerTiles, opt => opt.MapFrom<PlayerTilesResolver>());

            CreateMap<RoundPlayer, RoundOtherPlayerDto>()
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(s => s.AppUser.DisplayName))
                .ForMember(dest => dest.HasAction, opt => opt.MapFrom(s => s.RoundPlayerActions.Count() > 0))
                .ForMember(dest => dest.ActiveTilesCount, opt => opt.MapFrom<ActiveTilesCountResolver>())
                .ForMember(dest => dest.GraveyardTiles, opt => opt.MapFrom<GraveyardTilesResolver>())
                .ForMember(dest => dest.SeatOrientation, opt => opt.MapFrom<SeatOrientationResolver>());

            CreateMap<RoundResult, RoundResultDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(s => s.AppUser.UserName))
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(s => s.AppUser.DisplayName));
        }
    }
}
