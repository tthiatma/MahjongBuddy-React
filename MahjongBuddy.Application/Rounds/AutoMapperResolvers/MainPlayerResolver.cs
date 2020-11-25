using AutoMapper;
using MahjongBuddy.Application.Dtos;
using MahjongBuddy.Application.Interfaces;
using MahjongBuddy.Core;
using System.Linq;

namespace MahjongBuddy.Application.Rounds.AutoMapperResolvers
{
    public class MainPlayerResolver : IValueResolver<Round, RoundDto, RoundPlayerDto>
    {
        private readonly IMapper _mapper;
        private readonly IUserAccessor _userAccessor;

        public MainPlayerResolver(IMapper mapper, IUserAccessor userAccessor)
        {
            _mapper = mapper;
            _userAccessor = userAccessor;
        }

        public RoundPlayerDto Resolve(Round source, RoundDto destination, RoundPlayerDto destMember, ResolutionContext context)
        {
            var roundId = source.Id;
            var mainPlayerUserName = string.Empty;
            if (context.Options.Items.Count() > 0)
            {
                if(context.Options.Items.ContainsKey("MainRoundPlayer"))
                {
                    var rp = context.Items["MainRoundPlayer"] as RoundPlayer;
                    mainPlayerUserName = rp.GamePlayer.AppUser.UserName;
                }
            }
            else
            {
                mainPlayerUserName = _userAccessor.GetCurrentUserName();
            }
            var mainPlayer = source.RoundPlayers.First(rp => rp.GamePlayer.AppUser.UserName == mainPlayerUserName);
            return _mapper.Map<RoundPlayer, RoundPlayerDto>(mainPlayer);
        }
    }
}
