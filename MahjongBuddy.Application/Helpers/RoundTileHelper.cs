using MahjongBuddy.Core;
using MahjongBuddy.EntityFramework.EntityFramework;
using System.Collections.Generic;
using System.Linq;

namespace MahjongBuddy.Application.Helpers
{
    public static class RoundTileHelper
    {
        public static void AssignTilesToUser(int tilesCount, string userId, IEnumerable<RoundTile> roundTiles)
        {
            var newTiles = roundTiles.Where(x => string.IsNullOrEmpty(x.Owner));
            int x = 0;
            foreach (var playTile in newTiles)
            {
                playTile.Owner = userId;
                if (playTile.Tile.TileType == TileType.Flower)
                {
                    playTile.Status = TileStatus.UserGraveyard;
                }
                else
                {
                    playTile.Status = TileStatus.UserActive;
                    x++;
                }
                if (x == tilesCount)
                    return;
            }
        }

        public static void ResetTiles(MahjongBuddyDbContext context)
        { 
        
        }


        public static List<RoundTile>CreateTiles(MahjongBuddyDbContext context)
        {
            var allTiles = context.Tiles.ToList();
            List<RoundTile> tiles = new List<RoundTile>();

            for (var i = 1; i < 5; i++)
            {
                tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 1) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 2) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 3) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 4) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 5) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 6) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 7) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 8) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 9) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 11) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 12) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 13) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 14) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 15) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 16) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 17) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 18) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 19) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 21) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 22) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 23) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 24) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 25) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 26) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 27) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 28) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 29) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 31) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 32) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 33) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 41) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 42) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 43) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 44) });
            };

            tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 51) });
            tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 52) });
            tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 53) });
            tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 54) });

            tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 61) });
            tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 62) });
            tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 63) });
            tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 64) });

            return tiles;
        }
    }
}
