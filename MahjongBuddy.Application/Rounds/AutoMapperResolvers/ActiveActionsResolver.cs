using AutoMapper;
using MahjongBuddy.Application.Dtos;
using MahjongBuddy.Core;
using MahjongBuddy.EntityFramework.EntityFramework;
using System.Collections.Generic;
using System.Linq;
namespace MahjongBuddy.Application.Rounds.AutoMapperResolvers
{
    public class ActiveActionsResolver : IValueResolver<RoundPlayer, RoundPlayerDto, ICollection<RoundPlayerActionDto>>
    {
        private readonly IMapper _mapper;
        private readonly MahjongBuddyDbContext _context;

        public ActiveActionsResolver(IMapper mapper, MahjongBuddyDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public ICollection<RoundPlayerActionDto> Resolve(RoundPlayer source, RoundPlayerDto destination, ICollection<RoundPlayerActionDto> destMember, ResolutionContext context)
        {
            var roundId = source.RoundId;
            var activeActions = source.RoundPlayerActions.Where(a => a.ActionStatus == ActionStatus.Active);
            var dtoresult = _mapper.Map<ICollection<RoundPlayerAction>, ICollection<RoundPlayerActionDto>>(activeActions.ToList());
            return dtoresult;
        }
    }
}
