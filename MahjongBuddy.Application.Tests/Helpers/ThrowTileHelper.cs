using MahjongBuddy.Core;
using MahjongBuddy.EntityFramework.EntityFramework;
using System.Linq;
using System.Collections.Generic;

namespace MahjongBuddy.Application.Tests.Helpers
{
    public static class ThrowTileHelper
    {
        public static RoundTile SetupForPong(MahjongBuddyDbContext context, string userId)
        {
            List<RoundTile> userTiles = new List<RoundTile>();

            var round = context.Rounds.First();

            var oneCircle = round.RoundTiles.First(t => t.Tile.TileValue == TileValue.One && t.Tile.TileType == TileType.Circle);
            oneCircle.TileSetGroup = TileSetGroup.Chow;
            userTiles.Add(oneCircle);

            var twoCircle = round.RoundTiles.First(t => t.Tile.TileValue == TileValue.Two && t.Tile.TileType == TileType.Circle);
            oneCircle.TileSetGroup = TileSetGroup.Chow;
            userTiles.Add(twoCircle);

            var threeCircle = round.RoundTiles.First(t => t.Tile.TileValue == TileValue.Three && t.Tile.TileType == TileType.Circle);
            oneCircle.TileSetGroup = TileSetGroup.Chow;
            userTiles.Add(threeCircle);

            userTiles.Add(round.RoundTiles.First(t => t.Tile.TileValue == TileValue.Four && t.Tile.TileType == TileType.Circle));
            userTiles.Add(round.RoundTiles.First(t => t.Tile.TileValue == TileValue.Five && t.Tile.TileType == TileType.Circle));
            userTiles.Add(round.RoundTiles.First(t => t.Tile.TileValue == TileValue.Six && t.Tile.TileType == TileType.Circle));

            userTiles.Add(round.RoundTiles.First(t => t.Tile.TileValue == TileValue.One && t.Tile.TileType == TileType.Money));
            userTiles.Add(round.RoundTiles.First(t => t.Tile.TileValue == TileValue.Two && t.Tile.TileType == TileType.Money));
            userTiles.Add(round.RoundTiles.First(t => t.Tile.TileValue == TileValue.Three && t.Tile.TileType == TileType.Money));

            userTiles.Add(round.RoundTiles.First(t => t.Tile.TileValue == TileValue.WindEast));
            userTiles.Add(round.RoundTiles.First(t => t.Tile.TileValue == TileValue.WindWest));

            userTiles.AddRange(context.RoundTiles.Where(t => t.Tile.TileValue == TileValue.Six && t.Tile.TileType == TileType.Money).Take(2));

            foreach (var t in userTiles)
            {
                t.Owner = userId;
                t.Status = TileStatus.UserActive;
            }

            var chowTile = round.RoundTiles.First(t => t.Tile.TileValue == TileValue.Six && t.Tile.TileType == TileType.Money && string.IsNullOrEmpty(t.Owner));
            chowTile.Owner = "south";
            chowTile.Status = TileStatus.UserActive;

            context.SaveChanges();

            return chowTile;
        }
    }
}
