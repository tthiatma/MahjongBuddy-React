using MahjongBuddy.Core;
using System.Collections.Generic;
using System.Linq;

namespace MahjongBuddy.Application.Rounds.Scorings.HandTypes
{
    class Dragon : FindHandType
    {
        public override List<HandType> HandleRequest(IEnumerable<RoundTile> tiles, List<HandType> handTypes)
        {
            if (tiles == null)
                return handTypes;


            var redDragonTiles = tiles.Where(t => t.Tile.TileValue == TileValue.DragonRed);
            var greenDragonTiles = tiles.Where(t => t.Tile.TileValue == TileValue.DragonGreen);
            var whiteDragonTiles = tiles.Where(t => t.Tile.TileValue == TileValue.DragonWhite);

            //check for big dragon
            //for big dragon, user need to have minimum of 3 red 3 green and 3 white
            if (redDragonTiles.Count() >= 3
                && greenDragonTiles.Count() >= 3
                && whiteDragonTiles.Count() >= 3)
                handTypes.Add(HandType.BigDragon);

            //check for small dragon
            //for small dragon, user need to have minimum of two sets of dragon and eye with dragon 
            //first find dragon that act as eye
            var dragonTilesOnly = tiles
                .Where(t => t.Tile.TileType == TileType.Dragon);

            IEnumerable<TileValue> dragonEye = dragonTilesOnly
                .GroupBy(t => t.Tile.TileValue)
                .Where(grp => grp.Count() == 2)
                .Select(grp => grp.Key);

            if (dragonEye != null && dragonEye.Count() > 0)
            {
                var theEye = dragonEye.First();
                var otherDragonTiles = dragonTilesOnly
                    .Where(t => t.Tile.TileValue != theEye)
                    .GroupBy(t => t.Tile.TileValue)
                    .Where(grp => grp.Count() >= 3)
                    .Select(grp => grp.Key);

                if(otherDragonTiles.Count() == 2)
                    handTypes.Add(HandType.SmallDragon);
            }

            if (_successor != null)
                return _successor.HandleRequest(tiles, handTypes);
            else
                return handTypes;
        }
    }
}
