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
                tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Money && x.TileValue == TileValue.One) });
                tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Money && x.TileValue == TileValue.Two) });
                tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Money && x.TileValue == TileValue.Three) });
                tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Money && x.TileValue == TileValue.Four) });
                tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Money && x.TileValue == TileValue.Five) });
                tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Money && x.TileValue == TileValue.Six) });
                tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Money && x.TileValue == TileValue.Seven) });
                tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Money && x.TileValue == TileValue.Eight) });
                tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Money && x.TileValue == TileValue.Nine) });

                tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Circle && x.TileValue == TileValue.One) });
                tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Circle && x.TileValue == TileValue.Two) });
                tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Circle && x.TileValue == TileValue.Three) });
                tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Circle && x.TileValue == TileValue.Four) });
                tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Circle && x.TileValue == TileValue.Five) });
                tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Circle && x.TileValue == TileValue.Six) });
                tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Circle && x.TileValue == TileValue.Seven) });
                tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Circle && x.TileValue == TileValue.Eight) });
                tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Circle && x.TileValue == TileValue.Nine) });

                tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Stick && x.TileValue == TileValue.One) });
                tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Stick && x.TileValue == TileValue.Two) });
                tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Stick && x.TileValue == TileValue.Three) });
                tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Stick && x.TileValue == TileValue.Four) });
                tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Stick && x.TileValue == TileValue.Five) });
                tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Stick && x.TileValue == TileValue.Six) });
                tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Stick && x.TileValue == TileValue.Seven) });
                tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Stick && x.TileValue == TileValue.Eight) });
                tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Stick && x.TileValue == TileValue.Nine) });

                tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Dragon && x.TileValue == TileValue.DragonGreen) });
                tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Dragon && x.TileValue == TileValue.DragonRed) });
                tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Dragon && x.TileValue == TileValue.DragonWhite) });

                tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Wind && x.TileValue == TileValue.WindEast) });
                tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Wind && x.TileValue == TileValue.WindSouth) });
                tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Wind && x.TileValue == TileValue.WindWest) });
                tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Wind && x.TileValue == TileValue.WindNorth) });

            };

            tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Flower && x.TileValue == TileValue.FlowerRomanOne) });
            tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Flower && x.TileValue == TileValue.FlowerRomanTwo) });
            tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Flower && x.TileValue == TileValue.FlowerRomanThree) });
            tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Flower && x.TileValue == TileValue.FlowerRomanFour) });

            tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Flower && x.TileValue == TileValue.FlowerNumericOne) });
            tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Flower && x.TileValue == TileValue.FlowerNumericTwo) });
            tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Flower && x.TileValue == TileValue.FlowerNumericThree) });
            tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Flower && x.TileValue == TileValue.FlowerNumericFour) });

            return tiles;
        }

        public static HandType DetermineHandCanWin(IEnumerable<RoundTile> tiles)
        {
            //TODO leverage tile that already in user graveyard in determining user hand type

            //if all weird combo hand is checked, in order to win 
            //tiles needs to be either, chicken, straight, or triplet

            var listTiles = tiles.ToList();
            //check if there's kong tiles, because the total tiles will be more than 14
            //we want to take off 1 kong tiles to make it total of 14 tiles to easily determine valid tiles to win
            var kongTiles = tiles
                .Where(t => t.TileSetGroup == TileSetGroup.Kong)
                .GroupBy(t => new { t.Tile.TileType, t.Tile.TileValue });

            foreach (var group in kongTiles)
            {
                //take off 1 tile
                listTiles.Remove(group.First());
            }
            tiles = listTiles;
            if (tiles.Count() != 14)
                return HandType.None;

            //get possible eyes
            List<IEnumerable<RoundTile>> eyeCollection = new List<IEnumerable<RoundTile>>();
            foreach (var t in tiles)
            {
                var sameTiles = tiles.Where(ti => ti.Tile.TileValue == t.Tile.TileValue 
                && ti.Tile.TileType == t.Tile.TileType);

                if (sameTiles != null && sameTiles.Count() > 1)
                {
                    bool exist = false;
                    var tv = sameTiles.First().Tile.TileValue;
                    var tp = sameTiles.First().Tile.TileType;
                    foreach (IEnumerable<RoundTile> e in eyeCollection)
                    {
                        if (e.Where(t => t.Tile.TileType == tp && t.Tile.TileValue == tv).Count() > 0)
                        {
                            exist = true;
                            break;
                        }
                    }
                    if(!exist)
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

            //if there are 3+ wind or 3+ dragon, then its not possible for the tiless to be straight
            var windOrDragonTiles = tiles.Where(t => t.Tile.TileType == TileType.Dragon || t.Tile.TileType == TileType.Wind);

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
                if (temp.Count() == 3)
                    return temp;
            }
            return null;
        }

        public static IEnumerable<RoundTile> GetAnySet(IEnumerable<RoundTile> tiles)
        {
            foreach (var t in tiles)
            {
                var temp = FindPongTiles(t, tiles);
                if (temp.Count() == 0)
                    temp = FindStraightTiles(t, tiles);
                if (temp != null && temp.Count() == 3)
                    return temp;
            }
            return null;
        }

        public static IEnumerable<RoundTile> GetStraightSet(IEnumerable<RoundTile> tiles)
        {
            foreach (var t in tiles)
            {
                if (t.Tile.TileType == TileType.Dragon || t.Tile.TileType == TileType.Wind)
                    return null;

                var temp = FindStraightTiles(t, tiles);
                if (temp.Count() == 3)
                    return temp;
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
