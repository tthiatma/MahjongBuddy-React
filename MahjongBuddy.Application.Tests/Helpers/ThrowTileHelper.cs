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

            var pongTile = round.RoundTiles.First(t => t.Tile.TileValue == TileValue.Six && t.Tile.TileType == TileType.Money && string.IsNullOrEmpty(t.Owner));
            pongTile.Owner = "south";
            pongTile.Status = TileStatus.UserActive;

            context.SaveChanges();

            return pongTile;
        }

        public static RoundTile SetupForKong(MahjongBuddyDbContext context, string userId)
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

            userTiles.AddRange(context.RoundTiles.Where(t => t.Tile.TileValue == TileValue.Six && t.Tile.TileType == TileType.Money).Take(3));

            foreach (var t in userTiles)
            {
                t.Owner = userId;
                t.Status = TileStatus.UserActive;
            }

            var kongTile = round.RoundTiles.First(t => t.Tile.TileValue == TileValue.Six && t.Tile.TileType == TileType.Money && string.IsNullOrEmpty(t.Owner));
            kongTile.Owner = "south";
            kongTile.Status = TileStatus.UserActive;

            context.SaveChanges();

            return kongTile;
        }

        public static RoundTile SetupForWin(MahjongBuddyDbContext context, string userId)
        {
            List<RoundTile> userTiles = new List<RoundTile>();

            var round = context.Rounds.First();

            userTiles.Add(round.RoundTiles.First(t => t.Tile.TileValue == TileValue.One && t.Tile.TileType == TileType.Circle));
            userTiles.Add(round.RoundTiles.First(t => t.Tile.TileValue == TileValue.Two && t.Tile.TileType == TileType.Circle));
            userTiles.Add(round.RoundTiles.First(t => t.Tile.TileValue == TileValue.Three && t.Tile.TileType == TileType.Circle));

            userTiles.Add(round.RoundTiles.First(t => t.Tile.TileValue == TileValue.Four && t.Tile.TileType == TileType.Circle));
            userTiles.Add(round.RoundTiles.First(t => t.Tile.TileValue == TileValue.Five && t.Tile.TileType == TileType.Circle));
            userTiles.Add(round.RoundTiles.First(t => t.Tile.TileValue == TileValue.Six && t.Tile.TileType == TileType.Circle));

            userTiles.Add(round.RoundTiles.First(t => t.Tile.TileValue == TileValue.One && t.Tile.TileType == TileType.Money));
            userTiles.Add(round.RoundTiles.First(t => t.Tile.TileValue == TileValue.Two && t.Tile.TileType == TileType.Money));
            userTiles.Add(round.RoundTiles.First(t => t.Tile.TileValue == TileValue.Three && t.Tile.TileType == TileType.Money));

            userTiles.Add(round.RoundTiles.First(t => t.Tile.TileValue == TileValue.Four && t.Tile.TileType == TileType.Money));
            userTiles.Add(round.RoundTiles.First(t => t.Tile.TileValue == TileValue.Five && t.Tile.TileType == TileType.Money));
            userTiles.Add(round.RoundTiles.First(t => t.Tile.TileValue == TileValue.Six && t.Tile.TileType == TileType.Money));
            
            var matchingTile = round.RoundTiles.First(t => t.Tile.TileValue == TileValue.WindEast);
            userTiles.Add(matchingTile);

            foreach (var t in userTiles)
            {
                t.Owner = userId;
                t.Status = TileStatus.UserActive;
            }

            var throwToWinTile = round.RoundTiles.Last(t => t.Tile.TileValue == TileValue.WindEast && t.Id != matchingTile.Id);
            throwToWinTile.Owner = "south";
            throwToWinTile.Status = TileStatus.UserActive;

            context.SaveChanges();

            return throwToWinTile;
        }

        public static RoundTile SetupForChow(MahjongBuddyDbContext context, string userId)
        {
            List<RoundTile> userTiles = new List<RoundTile>();

            var round = context.Rounds.First();

            userTiles.Add(round.RoundTiles.First(t => t.Tile.TileValue == TileValue.One && t.Tile.TileType == TileType.Circle));
            userTiles.Add(round.RoundTiles.First(t => t.Tile.TileValue == TileValue.Two && t.Tile.TileType == TileType.Circle));
            userTiles.Add(round.RoundTiles.First(t => t.Tile.TileValue == TileValue.Three && t.Tile.TileType == TileType.Circle));

            userTiles.Add(round.RoundTiles.First(t => t.Tile.TileValue == TileValue.Four && t.Tile.TileType == TileType.Circle));
            userTiles.Add(round.RoundTiles.First(t => t.Tile.TileValue == TileValue.Five && t.Tile.TileType == TileType.Circle));
            userTiles.Add(round.RoundTiles.First(t => t.Tile.TileValue == TileValue.Six && t.Tile.TileType == TileType.Circle));

            userTiles.Add(round.RoundTiles.First(t => t.Tile.TileValue == TileValue.Two && t.Tile.TileType == TileType.Money));
            userTiles.Add(round.RoundTiles.First(t => t.Tile.TileValue == TileValue.Three && t.Tile.TileType == TileType.Money));
            userTiles.Add(round.RoundTiles.Last(t => t.Tile.TileValue == TileValue.Three && t.Tile.TileType == TileType.Money));

            userTiles.Add(round.RoundTiles.First(t => t.Tile.TileValue == TileValue.Five && t.Tile.TileType == TileType.Money));
            userTiles.Add(round.RoundTiles.First(t => t.Tile.TileValue == TileValue.Six && t.Tile.TileType == TileType.Money));

            userTiles.Add(round.RoundTiles.First(t => t.Tile.TileValue == TileValue.WindEast));
            userTiles.Add(round.RoundTiles.First(t => t.Tile.TileValue == TileValue.WindWest));

            foreach (var t in userTiles)
            {
                t.Owner = userId;
                t.Status = TileStatus.UserActive;
            }

            var throwChowTile = round.RoundTiles.First(t => t.Tile.TileValue == TileValue.Four && t.Tile.TileType == TileType.Money);
            throwChowTile.Owner = "south";
            throwChowTile.Status = TileStatus.UserActive;

            context.SaveChanges();

            return throwChowTile;
        }
    }
}
