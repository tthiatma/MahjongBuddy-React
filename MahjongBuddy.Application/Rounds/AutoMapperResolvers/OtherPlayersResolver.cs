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
        private readonly MahjongBuddyDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserAccessor _userAccessor;

        public OtherPlayersResolver(MahjongBuddyDbContext context, IMapper mapper, IUserAccessor userAccessor)
        {
            _context = context;
            _mapper = mapper;
            _userAccessor = userAccessor;
        }

        public ICollection<RoundOtherPlayerDto> Resolve(Round source, RoundDto destination, ICollection<RoundOtherPlayerDto> destMember, ResolutionContext context)
        {
            var roundId = source.Id;
            var otherPlayers = source.RoundPlayers.Where(rp => rp.AppUser.UserName != _userAccessor.GetCurrentUserName());
            return _mapper.Map<ICollection<RoundPlayer>, ICollection<RoundOtherPlayerDto>>(otherPlayers.ToList());
        }
    }
}
