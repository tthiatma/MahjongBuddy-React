using MahjongBuddy.Application.Extensions;
using MahjongBuddy.Core;
using MahjongBuddy.Core.Enums;
using System.Collections.Generic;
using System.Linq;

namespace MahjongBuddy.Application.Rounds.Scorings.ExtraPoints
{
    class ExtraWind : FindExtraPoint
    {
        public override List<ExtraPoint> HandleRequest(IEnumerable<RoundTile> tiles, Round round, List<ExtraPoint> extraPoints)
        {
            //if this is user's wind



            //if this is prevailing wind
            var currentWind = round.Wind.ToTileValue();
            var prevailingWinds = tiles.Where(t => t.Tile.TileValue == currentWind);
            if (prevailingWinds.Count() >= 3)
                extraPoints.Add(ExtraPoint.PrevailingWind);

            if (_successor != null)
                return _successor.HandleRequest(tiles, round, extraPoints);
            else
                return extraPoints;

        }
    }
}
