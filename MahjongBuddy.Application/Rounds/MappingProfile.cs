using AutoMapper;
using MahjongBuddy.Application.Dtos;
using MahjongBuddy.Core;

namespace MahjongBuddy.Application.Rounds
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<Round, RoundDto>();
            CreateMap<RoundTile, RoundTileDto>();
            CreateMap<UserRound, RoundPlayerDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(s => s.AppUser.UserName))
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(s => s.AppUser.DisplayName));
        }
    }
}
