using MahjongBuddy.Core;
using MahjongBuddy.EntityFramework.EntityFramework;
using System.Linq;

namespace MahjongBuddy.Application.Tests.Helpers
{
    public static class KongTilesHelper
    {
        public static void SetupForBoard(MahjongBuddyDbContext context, string userId, TileType tileType, TileValue tileValue)
        {
            //setup tiles where user can kong from board active
            var tilesToKong = context.RoundTiles.Where(t => t.Tile.TileType == tileType && t.Tile.TileValue == tileValue);
            if(tilesToKong.Count() == 4)
            {
                foreach (var t in tilesToKong.Take(3))
                {
                    t.Owner = userId;
                    t.Status = TileStatus.UserActive;
                }

                var lastTile = tilesToKong.Last();
                lastTile.Owner = DefaultValue.board;
                lastTile.Status = TileStatus.BoardActive;
            }
        }

        public static void SetupForSelfActive(MahjongBuddyDbContext context, string userId, TileType tileType, TileValue tileValue)
        {
            //setup 4 same tiles as user active
            var oneRoundTiles = context.RoundTiles.Where(t => t.Tile.TileType == tileType && t.Tile.TileValue == tileValue);
            if (oneRoundTiles.Count() == 4)
            {
                foreach (var t in oneRoundTiles)
                {
                    t.Owner = userId;
                    t.Status = TileStatus.UserActive;
                }
            }
        }

        public static void SetupForPongUser(MahjongBuddyDbContext context, string userId, TileType tileType, TileValue tileValue)
        {
            //setup 3 same tiles as ponged in graveyard and 1 tile as user active
            var oneRoundTiles = context.RoundTiles.Where(t => t.Tile.TileType == tileType && t.Tile.TileValue == tileValue);
            if (oneRoundTiles.Count() == 4)
            {
                foreach (var t in oneRoundTiles.Take(3))
                {
                    t.Owner = userId;
                    t.Status = TileStatus.UserGraveyard;
                    t.TileSetGroup = TileSetGroup.Pong;
                }

                var lastTile = oneRoundTiles.Last();
                lastTile.Owner = userId;
                lastTile.Status = TileStatus.UserActive;
            }
        }
        public static void SetupForPongBoard(MahjongBuddyDbContext context, string userId, TileType tileType, TileValue tileValue)
        {
            //setup 3 same tiles as ponged in graveyard and 1 tile as board active
            var oneRoundTiles = context.RoundTiles.Where(t => t.Tile.TileType == tileType && t.Tile.TileValue == tileValue);
            if (oneRoundTiles.Count() == 4)
            {
                foreach (var t in oneRoundTiles.Take(3))
                {
                    t.Owner = userId;
                    t.Status = TileStatus.UserGraveyard;
                    t.TileSetGroup = TileSetGroup.Pong;
                }

                var lastTile = oneRoundTiles.Last();
                lastTile.Owner = "board";
                lastTile.Status = TileStatus.BoardActive;
            }
        }

        public static void SetupForChowUser(MahjongBuddyDbContext context, string userId, TileType tileType, TileValue tileValue)
        {
            //setup 3 same tiles as chow in graveyard and 1 tile as user active
            var oneRoundTiles = context.RoundTiles.Where(t => t.Tile.TileType == tileType && t.Tile.TileValue == tileValue);
            if (oneRoundTiles.Count() == 4)
            {
                foreach (var t in oneRoundTiles.Take(3))
                {
                    t.Owner = userId;
                    t.Status = TileStatus.UserGraveyard;
                    t.TileSetGroup = TileSetGroup.Chow;
                }

                var lastTile = oneRoundTiles.Last();
                lastTile.Owner = userId;
                lastTile.Status = TileStatus.UserActive;
            }
        }

        public static void SetupForChowBoard(MahjongBuddyDbContext context, string userId, TileType tileType, TileValue tileValue)
        {
            //setup 3 same tiles as chow in graveyard and 1 tile as board active
            var oneRoundTiles = context.RoundTiles.Where(t => t.Tile.TileType == tileType && t.Tile.TileValue == tileValue);
            if (oneRoundTiles.Count() == 4)
            {
                foreach (var t in oneRoundTiles.Take(3))
                {
                    t.Owner = userId;
                    t.Status = TileStatus.UserGraveyard;
                    t.TileSetGroup = TileSetGroup.Chow;
                }

                var lastTile = oneRoundTiles.Last();
                lastTile.Owner = "board";
                lastTile.Status = TileStatus.BoardActive;
            }
        }
    }
}
