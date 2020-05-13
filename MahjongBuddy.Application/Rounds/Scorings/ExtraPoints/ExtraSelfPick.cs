using MahjongBuddy.Application.Extensions;
using MahjongBuddy.Core;
using MahjongBuddy.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MahjongBuddy.Application.Rounds.Scorings.ExtraPoints
{
    class ExtraSelfPick : FindExtraPoint
    {
        public override List<ExtraPoint> HandleRequest(Round round, string winnerUserName, List<ExtraPoint> extraPoints)
        {
            var tiles = round.RoundTiles.Where(t => t.Owner == winnerUserName);
            var winner = round.RoundPlayers.FirstOrDefault(u => u.AppUser.UserName == winnerUserName);

            if (winner == null)
                throw new Exception("creating round not appropriately, winner need to be in the round");

            //if the tile is justpicked
            var justPickedTile = tiles.Where(t => t.Status == TileStatus.UserJustPicked);
            if (justPickedTile.Count() > 0)
                extraPoints.Add(ExtraPoint.SelfPick);

            if (_successor != null)
                return _successor.HandleRequest(round, winnerUserName, extraPoints);
            else
                return extraPoints;
        }
    }
}
