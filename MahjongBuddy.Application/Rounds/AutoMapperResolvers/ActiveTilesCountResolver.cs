using AutoMapper;
using MahjongBuddy.Application.Dtos;
using MahjongBuddy.Core;
using MahjongBuddy.EntityFramework.EntityFramework;
using System.Linq;
namespace MahjongBuddy.Application.Rounds.AutoMapperResolvers
{
    public class ActiveTilesCountResolver : IValueResolver<RoundPlayer, RoundOtherPlayerDto, int>
    {
        private readonly MahjongBuddyDbContext _context;

        public ActiveTilesCountResolver(MahjongBuddyDbContext context)
        {
            _context = context;
        }
        public int Resolve(RoundPlayer source, RoundOtherPlayerDto destination, int destMember, ResolutionContext context)
        {
            var roundId = source.RoundId;
            return _context.RoundTiles.Count(rt => rt.RoundId == roundId
            && rt.Owner == source.GamePlayer.Player.UserName && rt.Status == TileStatus.UserActive);
        }
    }
}
