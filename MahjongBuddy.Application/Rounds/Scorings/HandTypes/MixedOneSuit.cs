using MahjongBuddy.Core;
using System.Collections.Generic;
using System.Linq;

namespace MahjongBuddy.Application.Rounds.Scorings.HandTypes
{
    class MixedOneSuit : FindHandType
    {
        public override List<HandType> HandleRequest(IEnumerable<RoundTile> tiles, List<HandType> handTypes)
        {
            if (tiles == null)
                return handTypes;

            bool allSameType = true;

            //check if theres dragon or wind for mixed one suit
            var dragonOrWind = tiles.Where(t => t.Tile.TileType == TileType.Dragon || t.Tile.TileType == TileType.Wind);

            //check first tile type that's not dragon or wind
            //all tiles that are not dragon and wind need to have same type for mixed one suit
            var firstTileType = tiles.FirstOrDefault(t => t.Tile.TileType != TileType.Dragon && t.Tile.TileType != TileType.Wind);

            var tilesExceptDragonAndWind = tiles.Except(dragonOrWind);

            if(firstTileType != null)
            {
                var tileType = firstTileType.Tile.TileType;
                foreach (var t in tilesExceptDragonAndWind)
                {
                    if (t.Tile.TileType != tileType)
                    {
                        allSameType = false;
                        break;
                    }
                }
            }
            
            if(dragonOrWind.Count() > 0 && allSameType)
                handTypes.Add(HandType.MixedOneSuit);

            if (_successor != null)
                return _successor.HandleRequest(tiles, handTypes);
            else
                return handTypes;
        }
    }
}
