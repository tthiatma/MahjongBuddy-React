using MahjongBuddy.Application.Helpers;
using MahjongBuddy.Core;
using System.Collections.Generic;
using System.Linq;

namespace MahjongBuddy.Application.Rounds.Scorings
{
    class ThirteenOrphans : FindHandType
    {
        public override List<HandType> HandleRequest(IEnumerable<RoundTile> tiles, List<HandType> handTypes)
        {
            var thirteenWonderTiles = TileHelper.BuildThirteenWonder();
            bool foundEyesFor13Wonders = false;
            bool is13Wonders = true;
            foreach (var t in thirteenWonderTiles)
            {
                var dTile = tiles.Where(tt => (tt.Tile.TileType == t.TileType) && (tt.Tile.TileValue == t.TileValue));

                if (dTile != null && dTile.Count() > 0)
                {
                    if (foundEyesFor13Wonders)
                    {
                        if (dTile.Count() != 1)
                        {
                            is13Wonders = false;
                            break;
                        }
                    }
                    else
                    {
                        if (dTile.Count() == 2)
                        {
                            foundEyesFor13Wonders = true;
                        }
                        else if (dTile.Count() > 2)
                        {
                            is13Wonders = false;
                            break;
                        }
                    }
                }
                else
                {
                    is13Wonders = false;
                    break;
                }
            }
            if (is13Wonders)
            {
                //short circuit
                handTypes.Add(HandType.ThirteenOrphans);
                return handTypes;
            }
            else
            {
                if (_successor != null)
                {
                    return _successor.HandleRequest(tiles, handTypes);
                }
                else
                {
                    return handTypes;
                }
            }
        }
    }
}
