using AutoMapper;
using MahjongBuddy.Application.Dtos;
using MahjongBuddy.Application.Interfaces;
using MahjongBuddy.Core;
using MahjongBuddy.EntityFramework.EntityFramework;
using System.Collections.Generic;
using System.Linq;

namespace MahjongBuddy.Application.Rounds.AutoMapperResolvers
{
    public class OtherPlayersResolver : IValueResolver<Round, RoundDto, ICollection<RoundOtherPlayerDto>>
    {
        private readonly IMapper _mapper;
        private readonly IUserAccessor _userAccessor;

        public OtherPlayersResolver(MahjongBuddyDbContext context, IMapper mapper, IUserAccessor userAccessor)
        {
            _mapper = mapper;
            _userAccessor = userAccessor;
        }

        public ICollection<RoundOtherPlayerDto> Resolve(Round source, RoundDto destination, ICollection<RoundOtherPlayerDto> destMember, ResolutionContext context)
        {
            var roundId = source.Id;
            var mainPlayerUserName = string.Empty;
            if (context.Options.Items.Count() > 0 && context.Options.Items.ContainsKey("MainRoundPlayer"))
            {
                var rp = context.Items["MainRoundPlayer"] as RoundPlayer;
                mainPlayerUserName = rp.GamePlayer.Player.UserName;
                var otherPlayers = source.RoundPlayers.Where(rp => rp.GamePlayer.Player.UserName != mainPlayerUserName);
                return _mapper.Map<ICollection<RoundPlayer>, ICollection<RoundOtherPlayerDto>>(otherPlayers.ToList(), opt => opt.Items["MainRoundPlayer"] = rp);

            }
            else
            {
                mainPlayerUserName = _userAccessor.GetCurrentUserName();
                var otherPlayers = source.RoundPlayers.Where(rp => rp.GamePlayer.Player.UserName != mainPlayerUserName);
                return _mapper.Map<ICollection<RoundPlayer>, ICollection<RoundOtherPlayerDto>>(otherPlayers.ToList());
            }
        }
    }
}
