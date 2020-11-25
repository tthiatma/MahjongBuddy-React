using AutoMapper;
using MahjongBuddy.Application.Dtos;
using MahjongBuddy.Core;
using MahjongBuddy.EntityFramework.EntityFramework;
using System.Collections.Generic;
using System.Linq;

namespace MahjongBuddy.Application.Rounds.AutoMapperResolvers
{
    public class GraveyardTilesResolver : IValueResolver<RoundPlayer, RoundOtherPlayerDto, ICollection<RoundTileDto>>
    {
        private readonly MahjongBuddyDbContext _context;
        private readonly IMapper _mapper;

        public GraveyardTilesResolver(MahjongBuddyDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public ICollection<RoundTileDto> Resolve(RoundPlayer source, RoundOtherPlayerDto destination, ICollection<RoundTileDto> destMember, ResolutionContext context)
        {
            var roundId = source.RoundId;
            var graveyardTiles = _context.RoundTiles.Where(rt => rt.RoundId == roundId
            && rt.Owner == source.GamePlayer.AppUser.UserName && rt.Status == TileStatus.UserGraveyard);
            return _mapper.Map<ICollection<RoundTile>, ICollection<RoundTileDto>>(graveyardTiles.ToList());
        }
    }
}
