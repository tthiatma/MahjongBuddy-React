using MahjongBuddy.Core;
using MahjongBuddy.Core.Enums;
using System.Collections.Generic;
using System.Linq;

namespace MahjongBuddy.Application.Rounds.Scorings.ExtraPoints
{
    class ExtraDragon : FindExtraPoint
    {
        public override List<ExtraPoint> HandleRequest(IEnumerable<RoundTile> tiles, Round round, List<ExtraPoint> extraPoints)
        {
            var redDragonTiles = tiles.Where(t => t.Tile.TileValue == TileValue.DragonRed);
            if (redDragonTiles.Count() >= 3)
                extraPoints.Add(ExtraPoint.RedDragon);

            var greenDragonTiles = tiles.Where(t => t.Tile.TileValue == TileValue.DragonGreen);
            if (greenDragonTiles.Count() >= 3)
                extraPoints.Add(ExtraPoint.GreenDragon);


            var whiteDragonTiles = tiles.Where(t => t.Tile.TileValue == TileValue.DragonWhite);
            if (whiteDragonTiles.Count() >= 3)
                extraPoints.Add(ExtraPoint.WhiteDragon);

            if (_successor != null)
                return _successor.HandleRequest(tiles, round, extraPoints);
            else
                return extraPoints;

        }
    }
}
