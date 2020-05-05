using MahjongBuddy.Core;
using MahjongBuddy.Core.Enums;
using System.Collections.Generic;
using System.Linq;

namespace MahjongBuddy.Application.Rounds.Scorings.ExtraPoints
{
    class ExtraDragon : FindExtraPoint
    {
        public override List<ExtraPoint> HandleRequest(Round round, string winnerUserName, List<ExtraPoint> extraPoints)
        {
            var tiles = round.RoundTiles.Where(t => t.Owner == winnerUserName);
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
                return _successor.HandleRequest(round, winnerUserName, extraPoints);
            else
                return extraPoints;

        }
    }
}
