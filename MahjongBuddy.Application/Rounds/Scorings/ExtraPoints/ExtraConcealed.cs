using MahjongBuddy.Core;
using MahjongBuddy.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MahjongBuddy.Application.Rounds.Scorings.ExtraPoints
{
    class ExtraConcealed : FindExtraPoint
    {
        public override List<ExtraPoint> HandleRequest(Round round, string winnerUserName, List<ExtraPoint> extraPoints)
        {
            var tiles = round.RoundTiles.Where(t => t.Owner == winnerUserName);
            var winner = round.UserRounds.FirstOrDefault(u => u.AppUser.UserName == winnerUserName);

            if (winner == null)
                throw new Exception("creating round not appropriately, winner need to be in the round");

            var hasOpenTiles = tiles.Any(t => t.TileSetGroup == TileSetGroup.Chow
            || t.TileSetGroup == TileSetGroup.Pong
            || t.TileSetGroup == TileSetGroup.Kong);

            if (!hasOpenTiles)
                extraPoints.Add(ExtraPoint.ConcealedHand);

            if (_successor != null)
                return _successor.HandleRequest(round, winnerUserName, extraPoints);
            else
                return extraPoints;
        }
    }
}
