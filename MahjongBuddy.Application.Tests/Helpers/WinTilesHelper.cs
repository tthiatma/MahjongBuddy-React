using MahjongBuddy.Core;
using MahjongBuddy.EntityFramework.EntityFramework;
using System.Linq;
using System.Collections.Generic;

namespace MahjongBuddy.Application.Tests.Helpers
{
    public static class WinTilesHelper
    {
        public static IEnumerable<RoundTile> SetupForSevenPairsWithBoard(MahjongBuddyDbContext context, string userId)
        {
            List<RoundTile> userTiles = new List<RoundTile>();

            var twoRedDragon = context.RoundTiles.Where(t => t.Tile.TileValue == TileValue.DragonRed).Take(2);
            userTiles.AddRange(twoRedDragon);

            var twoSticks = context.RoundTiles.Where(t => t.Tile.TileType == TileType.Stick && t.Tile.TileValue == TileValue.Eight).Take(2);
            userTiles.AddRange(twoSticks);

            var fourEast = context.RoundTiles.Where(t => t.Tile.TileValue == TileValue.WindEast);
            userTiles.AddRange(fourEast);

            var twoNorth = context.RoundTiles.Where(t => t.Tile.TileValue == TileValue.WindNorth).Take(2);
            userTiles.AddRange(twoNorth);

            var twoCircle = context.RoundTiles.Where(t => t.Tile.TileType == TileType.Circle && t.Tile.TileValue == TileValue.Eight).Take(2);
            userTiles.AddRange(twoCircle);

            var twoMoney = context.RoundTiles.Where(t => t.Tile.TileType == TileType.Money && t.Tile.TileValue == TileValue.Eight).Take(2);
            userTiles.AddRange(twoMoney);

            var oneGreenDragon = context.RoundTiles.First(t => t.Tile.TileValue == TileValue.DragonGreen);
            userTiles.Add(oneGreenDragon);

            foreach (var t in userTiles)
            {
                t.Owner = userId;
                t.Status = TileStatus.UserActive;
            }

            var boardTiles = context.RoundTiles.First(t => t.Tile.TileValue == TileValue.DragonGreen && string.IsNullOrEmpty(t.Owner));

            boardTiles.Owner = "board";
            boardTiles.Status = TileStatus.BoardActive;

            userTiles.Add(boardTiles);

            return userTiles;
        }

        public static IEnumerable<RoundTile> SetupForThirteenOrphansWithBoard(MahjongBuddyDbContext context, string userId)
        {
            //setup tiles where user have thirteen orphans
            List<RoundTile> userTiles = new List<RoundTile>();
            var circleTiles = context.RoundTiles.Where(t => t.Tile.TileType == TileType.Circle);
            userTiles.Add(circleTiles.First(t => t.Tile.TileValue == TileValue.One));
            userTiles.Add(circleTiles.First(t => t.Tile.TileValue == TileValue.Nine));

            var stickTiles = context.RoundTiles.Where(t => t.Tile.TileType == TileType.Stick);
            userTiles.Add(stickTiles.First(t => t.Tile.TileValue == TileValue.One));
            userTiles.Add(stickTiles.First(t => t.Tile.TileValue == TileValue.Nine));

            var moneyTiles = context.RoundTiles.Where(t => t.Tile.TileType == TileType.Money);
            userTiles.Add(moneyTiles.First(t => t.Tile.TileValue == TileValue.One));
            userTiles.Add(moneyTiles.First(t => t.Tile.TileValue == TileValue.Nine));

            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.DragonRed));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.DragonGreen));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.DragonWhite));

            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.WindEast));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.WindSouth));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.WindWest));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.WindNorth));

            foreach (var t in userTiles)
            {
                t.Owner = userId;
                t.Status = TileStatus.UserActive;
            }

            var boardTiles = context.RoundTiles.First(t => t.Tile.TileValue == TileValue.WindNorth && string.IsNullOrEmpty(t.Owner));

            boardTiles.Owner = "board";
            boardTiles.Status = TileStatus.BoardActive;

            userTiles.Add(boardTiles);

            return userTiles;
        }


    }
}
