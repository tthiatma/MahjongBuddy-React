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

        public static HandType DetermineHandCanWin(IEnumerable<RoundTile> tiles)
        {
            //if all weird combo hand is checked, in order to win 
            //tiles needs to be either, chicken, straight, or triplet

            if (tiles.Count() != 14)
                return HandType.None;

            //get possible eyes
            //TODO: improve the way picking eyes by selecting ignoring 3 same tiles first
            List<IEnumerable<RoundTile>> eyeCollection = new List<IEnumerable<RoundTile>>();
            foreach (var t in tiles)
            {
                var sameTiles = tiles.Where(ti => ti.Tile.TileValue == t.Tile.TileValue && ti.Tile.TileType == t.Tile.TileType);
                if (sameTiles != null && sameTiles.Count() > 1)
                {
                    eyeCollection.Add(sameTiles.Take(2));
                }
            }

            if (eyeCollection.Count() == 0)
                return HandType.None;

            //test for triplet first because it has higher point
            bool allPong = false;
            foreach (var eyes in eyeCollection)
            {
                //remove possible eyes from tiles
                var tilesWithoutEyes = tiles.OrderBy(t => t.Tile.TileValue).ToList();
                foreach (var t in eyes)
                {
                    tilesWithoutEyes.Remove(t);
                }

                //try check all pong
                for (int i = 0; i < 4; i++)
                {
                    var pongSet = GetPongSet(tilesWithoutEyes);
                    if(pongSet == null)
                    {
                        break;
                    }
                    tilesWithoutEyes = tilesWithoutEyes.Except(pongSet).ToList();
                    //if it gets all the way to last set
                    if (i == 3)
                        allPong = true;
                }
            }
            if (allPong)
                return HandType.Triplets;


            //test for straight
            bool allStraight = false;
            foreach (var eyes in eyeCollection)
            {
                //remove possible eyes from tiles
                var tilesWithoutEyes = tiles.OrderBy(t => t.Tile.TileValue).ToList();
                foreach (var t in eyes)
                {
                    tilesWithoutEyes.Remove(t);
                }

                //try check all straight
                for (int i = 0; i < 4; i++)
                {
                    var straightSet = GetStraightSet(tilesWithoutEyes);
                    if (straightSet == null)
                    {
                        break;
                    }
                    tilesWithoutEyes = tilesWithoutEyes.Except(straightSet).ToList();
                    //if it gets all the way to last set
                    if (i == 3)
                        allStraight = true;
                }
            }
            if (allStraight)
                return HandType.Straight;

            //test for chicken
            bool isChicken = false;
            foreach (var eyes in eyeCollection)
            {
                //remove possible eyes from tiles
                var tilesWithoutEyes = tiles.OrderBy(t => t.Tile.TileValue).ToList();
                foreach (var t in eyes)
                {
                    tilesWithoutEyes.Remove(t);
                }

                //try check all set
                for (int i = 0; i < 4; i++)
                {
                    var anySet = GetAnySet(tilesWithoutEyes);
                    if (anySet == null)
                    {
                        break;
                    }
                    tilesWithoutEyes = tilesWithoutEyes.Except(anySet).ToList();
                    //if it gets all the way to last set
                    if (i == 3)
                        isChicken = true;
                }
            }
            if (isChicken)
                return HandType.Chicken;

            return HandType.None;
        }


        public static IEnumerable<RoundTile> GetPongSet(IEnumerable<RoundTile> tiles)
        {
            foreach (var t in tiles)
            {
                var temp = FindPongTiles(t, tiles);
                if (temp != null && temp.Count() == 3)
                {
                    return temp;
                }
            }
            return null;
        }

        public static IEnumerable<RoundTile> GetAnySet(IEnumerable<RoundTile> tiles)
        {
            foreach (var t in tiles)
            {
                var temp = FindPongTiles(t, tiles);
                if (temp == null)
                    temp = FindStraightTiles(t, tiles);
                if (temp != null && temp.Count() == 3)
                {
                    return temp;
                }
            }
            return null;
        }

        public static IEnumerable<RoundTile> GetStraightSet(IEnumerable<RoundTile> tiles)
        {
            foreach (var t in tiles)
            {
                var temp = FindStraightTiles(t, tiles);
                if (temp != null && temp.Count() == 3)
                {
                    return temp;
                }
            }
            return null;
        }


        public static List<RoundTile> FindStraightTiles(RoundTile theTile, IEnumerable<RoundTile> tiles)
        {
            var ret = new List<RoundTile>();

            var sameTypeTiles = tiles.Where(t => t.Tile.TileType == theTile.Tile.TileType);
            foreach (var t in sameTypeTiles)
            {
                if (sameTypeTiles.Any(ti => ti.Tile.TileValue == (t.Tile.TileValue - 2)) && sameTypeTiles.Any(ti => ti.Tile.TileValue == (t.Tile.TileValue - 1)))
                {
                    ret.Add(t);
                    ret.Add(sameTypeTiles.Where(ti => ti.Tile.TileValue== (t.Tile.TileValue - 2)).First());
                    ret.Add(sameTypeTiles.Where(ti => ti.Tile.TileValue== (t.Tile.TileValue - 1)).First());
                    break;
                }

                if (sameTypeTiles.Any(ti => ti.Tile.TileValue== (t.Tile.TileValue - 1)) && sameTypeTiles.Any(ti => ti.Tile.TileValue== (t.Tile.TileValue + 1)))
                {
                    ret.Add(t);
                    ret.Add(sameTypeTiles.Where(ti => ti.Tile.TileValue== (t.Tile.TileValue - 1)).First());
                    ret.Add(sameTypeTiles.Where(ti => ti.Tile.TileValue== (t.Tile.TileValue + 1)).First());
                    break;
                }

                if (sameTypeTiles.Any(ti => ti.Tile.TileValue== (t.Tile.TileValue + 1)) && sameTypeTiles.Any(ti => ti.Tile.TileValue== (t.Tile.TileValue + 2)))
                {
                    ret.Add(t);
                    ret.Add(sameTypeTiles.Where(ti => ti.Tile.TileValue== (t.Tile.TileValue + 1)).First());
                    ret.Add(sameTypeTiles.Where(ti => ti.Tile.TileValue== (t.Tile.TileValue + 2)).First());
                    break;
                }
            }
            return ret;
        }


        public static List<RoundTile> FindPongTiles(RoundTile theTile, IEnumerable<RoundTile> tiles)
        {
            var ret = new List<RoundTile>();
            var sameTiles = tiles.Where(t => t.Tile.TileType == theTile.Tile.TileType && t.Tile.TileValue == theTile.Tile.TileValue);
            if (sameTiles != null && sameTiles.Count() == 3)
            {
                foreach (var t in sameTiles)
                {
                    ret.Add(t);
                }
            }
            return ret;
        }
    }
}
