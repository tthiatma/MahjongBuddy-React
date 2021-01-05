using AutoMapper;
using MahjongBuddy.Application.Dtos;
using MahjongBuddy.Core;
using MahjongBuddy.EntityFramework.EntityFramework;
using System.Collections.Generic;
using System.Linq;

namespace MahjongBuddy.Application.Rounds.AutoMapperResolvers
{
    public class ResultPlayerTilesResolver : IValueResolver<RoundResult, RoundResultDto, ICollection<RoundTileDto>>
    {
        private readonly MahjongBuddyDbContext _context;
        private readonly IMapper _mapper;

        public ResultPlayerTilesResolver(MahjongBuddyDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public ICollection<RoundTileDto> Resolve(RoundResult source, RoundResultDto destination, ICollection<RoundTileDto> destMember, ResolutionContext context)
        {
            var roundId = source.RoundId;
            var RoundPlayerTiles = _context.RoundTiles.Where(rt => rt.RoundId == roundId && rt.Owner == source.Player.UserName);
            return _mapper.Map<ICollection<RoundTile>, ICollection<RoundTileDto>>(RoundPlayerTiles.ToList());
        }
    }
}
