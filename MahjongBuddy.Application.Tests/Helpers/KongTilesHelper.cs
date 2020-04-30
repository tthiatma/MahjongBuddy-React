using MahjongBuddy.Core;
using MahjongBuddy.EntityFramework.EntityFramework;
using System.Linq;

namespace MahjongBuddy.Application.Tests.Helpers
{
    public static class KongTilesHelper
    {
        public static void KongFromActiveBoard(MahjongBuddyDbContext context, string userId, TileType tileType, TileValue tileValue)
        {
            //setup tiles where user can kong from board active

            var oneRoundTiles = context.RoundTiles.Where(t => t.Tile.TileType == tileType && t.Tile.TileValue == tileValue);
            if(oneRoundTiles.Count() == 4)
            {
                foreach (var t in oneRoundTiles.Take(3))
                {
                    t.Owner = userId;
                    t.Status = TileStatus.UserActive;
                }

                var lastTile = oneRoundTiles.Last();
                lastTile.Owner = "board";
                lastTile.Status = TileStatus.BoardActive;
            }
        }
    }
}
