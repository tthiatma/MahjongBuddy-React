using MahjongBuddy.Application.Helpers;
using MahjongBuddy.Core;
using System.Collections.Generic;
using System.Linq;

namespace MahjongBuddy.Application.Rounds.Scorings.HandTypes
{
    class Triplets : FindHandType
    {
        public Triplets(IEnumerable<RoundTile> tiles) : base(tiles) { }
        public override HandType HandleRequest(IEnumerable<RoundTile> tiles)
        {
            List<IEnumerable<RoundTile>> eyeCollection = new List<IEnumerable<RoundTile>>();
            foreach (var t in tiles)
            {
                var sameTiles = tiles.Where(ti => ti.Tile.TileValue == t.Tile.TileValue && ti.Tile.TileType == t.Tile.TileType);
                if (sameTiles != null && sameTiles.Count() > 1)
                {
                    eyeCollection.Add(sameTiles.Take(2));
                }
            }

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
                    var pongSet = RoundTileHelper.GetPongSet(tilesWithoutEyes);
                    if (pongSet == null)
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
            else
            {
                if (_successor != null)
                {
                    return _successor.HandleRequest(tiles);
                }
                else
                {
                    return HandType.None;
                }
            }
        }
    }
}
