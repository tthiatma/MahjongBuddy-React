using MahjongBuddy.Core;
using MahjongBuddy.EntityFramework.EntityFramework;
using System.Linq;
using System.Collections.Generic;

namespace MahjongBuddy.Application.Tests.Helpers
{
    public static class HomeGameCalculatorHelper
    {
        public static IEnumerable<RoundTile> SetupForStraight(MahjongBuddyDbContext context, string userId, bool selfPick)
        {
            List<RoundTile> userTiles = new List<RoundTile>();

            var oneCircle = context.RoundTiles.First(t => t.Tile.TileValue == TileValue.One && t.Tile.TileType == TileType.Circle);
            oneCircle.TileSetGroup = TileSetGroup.Chow;
            userTiles.Add(oneCircle);

            var twoCircle = context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Two && t.Tile.TileType == TileType.Circle);
            oneCircle.TileSetGroup = TileSetGroup.Chow;
            userTiles.Add(twoCircle);

            var threeCircle = context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Three && t.Tile.TileType == TileType.Circle);
            oneCircle.TileSetGroup = TileSetGroup.Chow;
            userTiles.Add(threeCircle);

            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Four && t.Tile.TileType == TileType.Circle));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Five && t.Tile.TileType == TileType.Circle));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Six && t.Tile.TileType == TileType.Circle));

            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.One && t.Tile.TileType == TileType.Money));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Two && t.Tile.TileType == TileType.Money));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Three && t.Tile.TileType == TileType.Money));

            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Four && t.Tile.TileType == TileType.Money));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Five && t.Tile.TileType == TileType.Money));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Six && t.Tile.TileType == TileType.Money));

            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.WindEast));

            foreach (var t in userTiles)
            {
                t.Owner = userId;
                t.Status = TileStatus.UserActive;
            }

            var lastTile = context.RoundTiles.Last(t => t.Tile.TileValue == TileValue.WindEast);

            if (selfPick)
            {
                lastTile.Owner = userId;
                lastTile.Status = TileStatus.UserJustPicked;
            }
            else
            {
                lastTile.Owner = "board";
                lastTile.Status = TileStatus.BoardActive;
            }

            userTiles.Add(lastTile);

            context.SaveChanges();

            return userTiles;
        }
    }
}
