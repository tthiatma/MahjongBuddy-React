using AutoMapper;
using MahjongBuddy.Application.Dtos;
using MahjongBuddy.Application.Rounds.AutoMapperResolvers;
using MahjongBuddy.Core;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq;

namespace MahjongBuddy.Application.Rounds
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<Round, RoundDto>()
                .ForMember(dest => dest.BoardTiles, opt => opt.MapFrom<BoardTilesResolver>())
                .ForMember(dest => dest.MainPlayer, opt => opt.MapFrom<MainPlayerResolver>())
                .ForMember(dest => dest.RemainingTiles, opt =>opt.MapFrom(s => s.RoundTiles.Count(t => string.IsNullOrEmpty(t.Owner))))
                .ForMember(dest => dest.OtherPlayers, opt => opt.MapFrom<OtherPlayersResolver>());

            CreateMap<RoundTile, RoundTileDto>();
            CreateMap<RoundResultHand, RoundResultHandDto>();
            CreateMap<RoundResultExtraPoint, RoundResultExtraPointDto>();
            CreateMap<RoundPlayerAction, RoundPlayerActionDto>();

            CreateMap<RoundPlayer, RoundPlayerDto>()
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(s => s.GamePlayer.Player.DisplayName))
                .ForMember(dest => dest.Connections, opt => opt.MapFrom(s => s.GamePlayer.Connections))
                .ForMember(dest => dest.PlayerTiles, opt => opt.MapFrom<PlayerTilesResolver>());

            CreateMap<RoundPlayer, RoundOtherPlayerDto>()
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(s => s.GamePlayer.Player.DisplayName))
                .ForMember(dest => dest.ActiveTilesCount, opt => opt.MapFrom<ActiveTilesCountResolver>())
                .ForMember(dest => dest.GraveyardTiles, opt => opt.MapFrom<GraveyardTilesResolver>())
                .ForMember(dest => dest.SeatOrientation, opt => opt.MapFrom<SeatOrientationResolver>());

            CreateMap<RoundResult, RoundResultDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(s => s.Player.UserName))
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(s => s.Player.DisplayName))
                .ForMember(dest => dest.PlayerTiles, opt => opt.MapFrom<ResultPlayerTilesResolver>());
        }
    }
}
