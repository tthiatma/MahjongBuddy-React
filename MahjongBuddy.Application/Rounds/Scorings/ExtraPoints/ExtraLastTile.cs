using MahjongBuddy.Application.Extensions;
using MahjongBuddy.Core;
using MahjongBuddy.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MahjongBuddy.Application.Rounds.Scorings.ExtraPoints
{
    class ExtraLastTile : FindExtraPoint
    {
        public override List<ExtraPoint> HandleRequest(Round round, string winnerUserName, List<ExtraPoint> extraPoints)
        {
            var allTiles = round.RoundTiles;
            var winner = round.RoundPlayers.FirstOrDefault(u => u.GamePlayer.Player.UserName == winnerUserName);

            if (winner == null)
                throw new Exception("creating round not appropriately, winner need to be in the round");

            var stillMoreTiles = allTiles.Any(t => string.IsNullOrEmpty(t.Owner));

            if (!stillMoreTiles && extraPoints.Contains(ExtraPoint.SelfPick))
                extraPoints.Add(ExtraPoint.WinOnLastTile);            

            if (_successor != null)
                return _successor.HandleRequest(round, winnerUserName, extraPoints);
            else
                return extraPoints;
        }
    }
}
