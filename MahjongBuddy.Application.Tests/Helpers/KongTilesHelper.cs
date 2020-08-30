using MahjongBuddy.Core;
using MahjongBuddy.EntityFramework.EntityFramework;
using System.Collections.Generic;
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
            List<RoundTile> userTiles = new List<RoundTile>();

            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Four && t.Tile.TileType == TileType.Circle));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Five && t.Tile.TileType == TileType.Circle));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Six && t.Tile.TileType == TileType.Circle));

            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.One && t.Tile.TileType == TileType.Money));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Two && t.Tile.TileType == TileType.Money));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Three && t.Tile.TileType == TileType.Money));

            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Four && t.Tile.TileType == TileType.Money));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Five && t.Tile.TileType == TileType.Money));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Six && t.Tile.TileType == TileType.Money));

            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Four && t.Tile.TileType == TileType.Stick));

            foreach (var t in userTiles)
            {
                t.Owner = userId;
                t.Status = TileStatus.UserActive;
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

            List<RoundTile> userTiles = new List<RoundTile>();

            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Four && t.Tile.TileType == TileType.Circle));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Five && t.Tile.TileType == TileType.Circle));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Six && t.Tile.TileType == TileType.Circle));

            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.One && t.Tile.TileType == TileType.Money));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Two && t.Tile.TileType == TileType.Money));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Three && t.Tile.TileType == TileType.Money));

            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Four && t.Tile.TileType == TileType.Money));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Five && t.Tile.TileType == TileType.Money));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Six && t.Tile.TileType == TileType.Money));

            foreach (var t in userTiles)
            {
                t.Owner = userId;
                t.Status = TileStatus.UserActive;
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

            List<RoundTile> userTiles = new List<RoundTile>();

            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Four && t.Tile.TileType == TileType.Circle));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Five && t.Tile.TileType == TileType.Circle));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Six && t.Tile.TileType == TileType.Circle));

            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.One && t.Tile.TileType == TileType.Money));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Two && t.Tile.TileType == TileType.Money));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Three && t.Tile.TileType == TileType.Money));

            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Four && t.Tile.TileType == TileType.Money));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Five && t.Tile.TileType == TileType.Money));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Six && t.Tile.TileType == TileType.Money));

            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Four && t.Tile.TileType == TileType.Stick));

            foreach (var t in userTiles)
            {
                t.Owner = userId;
                t.Status = TileStatus.UserActive;
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

            List<RoundTile> userTiles = new List<RoundTile>();

            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Four && t.Tile.TileType == TileType.Circle));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Five && t.Tile.TileType == TileType.Circle));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Six && t.Tile.TileType == TileType.Circle));

            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.One && t.Tile.TileType == TileType.Money));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Two && t.Tile.TileType == TileType.Money));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Three && t.Tile.TileType == TileType.Money));

            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Four && t.Tile.TileType == TileType.Money));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Five && t.Tile.TileType == TileType.Money));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Six && t.Tile.TileType == TileType.Money));

            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Four && t.Tile.TileType == TileType.Stick));

            foreach (var t in userTiles)
            {
                t.Owner = userId;
                t.Status = TileStatus.UserActive;
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
