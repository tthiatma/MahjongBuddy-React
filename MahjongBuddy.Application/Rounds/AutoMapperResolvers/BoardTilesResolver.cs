using AutoMapper;
using MahjongBuddy.Application.Dtos;
using MahjongBuddy.Core;
using MahjongBuddy.EntityFramework.EntityFramework;
using System.Collections.Generic;
using System.Linq;

namespace MahjongBuddy.Application.Rounds.AutoMapperResolvers
{
    public class BoardTilesResolver : IValueResolver<Round, RoundDto, ICollection<RoundTileDto>>
    {
        private readonly MahjongBuddyDbContext _context;
        private readonly IMapper _mapper;

        public BoardTilesResolver(MahjongBuddyDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public ICollection<RoundTileDto> Resolve(Round source, RoundDto destination, ICollection<RoundTileDto> destMember, ResolutionContext context)
        {
            var roundId = source.Id;
            var boardTiles = _context.RoundTiles.Where(rt => rt.RoundId == roundId && rt.Owner == DefaultValue.board);
            return _mapper.Map<ICollection<RoundTile>, ICollection<RoundTileDto>>(boardTiles.ToList());
        }
    }
}
