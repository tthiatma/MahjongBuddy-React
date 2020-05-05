using MahjongBuddy.Core;
using System.Collections.Generic;
using System.Linq;

namespace MahjongBuddy.Application.Rounds.Scorings.HandTypes
{
    class FourWind : FindHandType
    {
        public override List<HandType> HandleRequest(IEnumerable<RoundTile> tiles, List<HandType> handTypes)
        {
            if (tiles == null)
                return handTypes;

            var eastTiles = tiles.Where(t => t.Tile.TileValue == TileValue.WindEast);
            var southTiles = tiles.Where(t => t.Tile.TileValue == TileValue.WindSouth);
            var westTiles = tiles.Where(t => t.Tile.TileValue == TileValue.WindWest);
            var northTiles = tiles.Where(t => t.Tile.TileValue == TileValue.WindNorth);

            //check for big fourwind
            //for fourwind, user need to have minimum of all winds
            if (eastTiles.Count() >= 3
                && southTiles.Count() >= 3
                && westTiles.Count() >= 3
                && northTiles.Count() >= 3)
                handTypes.Add(HandType.BigFourWind);

            //check for small fourwind
            //for small fourwind, user need to have all winds but 1 of the wind as an eye 
            //first find wind that act as eye
            var windTilesOnly = tiles
                .Where(t => t.Tile.TileType == TileType.Wind);

            IEnumerable<TileValue> windEye = windTilesOnly
                .GroupBy(t => t.Tile.TileValue)
                .Where(grp => grp.Count() == 2)
                .Select(grp => grp.Key);

            if (windEye != null && windEye.Count() > 0)
            {
                var theEye = windEye.First();
                var otherWindTiles = windTilesOnly
                    .Where(t => t.Tile.TileValue != theEye)
                    .GroupBy(t => t.Tile.TileValue)
                    .Where(grp => grp.Count() >= 3)
                    .Select(grp => grp.Key);

                if (otherWindTiles.Count() == 3)
                    handTypes.Add(HandType.SmallFourWind);
            }

            if (_successor != null)
                return _successor.HandleRequest(tiles, handTypes);
            else
                return handTypes;
        }
    }
}
