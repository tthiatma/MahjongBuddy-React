using MahjongBuddy.Application.Extensions;
using MahjongBuddy.Core;
using MahjongBuddy.Core.Enums;
using Microsoft.EntityFrameworkCore.Internal;
using System.Collections.Generic;
using System.Linq;

namespace MahjongBuddy.Application.Rounds.Scorings.ExtraPoints
{
    class ExtraWind : FindExtraPoint
    {
        public override List<ExtraPoint> HandleRequest(Round round, string winnerUserName, List<ExtraPoint> extraPoints)
        {
            var tiles = round.RoundTiles;
            var winner = round.UserRounds.FirstOrDefault(u => u.AppUser.UserName == winnerUserName);

            //if this is user's wind
            var userCurrentWind = winner.Wind.ToTileValue();
            var userWind = tiles.Where(t => t.Tile.TileValue == userCurrentWind);
            if (userWind.Count() >= 3)
                extraPoints.Add(ExtraPoint.SeatWind);

            //if this is prevailing wind
            var currentWind = round.Wind.ToTileValue();
            var prevailingWinds = tiles.Where(t => t.Tile.TileValue == currentWind);
            if (prevailingWinds.Count() >= 3)
                extraPoints.Add(ExtraPoint.PrevailingWind);

            if (_successor != null)
                return _successor.HandleRequest(round, winnerUserName, extraPoints);
            else
                return extraPoints;

        }
    }
}
