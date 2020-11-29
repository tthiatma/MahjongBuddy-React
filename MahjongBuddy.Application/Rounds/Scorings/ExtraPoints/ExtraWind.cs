using MahjongBuddy.Application.Extensions;
using MahjongBuddy.Core;
using MahjongBuddy.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MahjongBuddy.Application.Rounds.Scorings.ExtraPoints
{
    class ExtraWind : FindExtraPoint
    {
        public override List<ExtraPoint> HandleRequest(Round round, string winnerUserName, List<ExtraPoint> extraPoints)
        {

            var tiles = round.RoundTiles.Where(t => t.Owner == winnerUserName);
            var winner = round.RoundPlayers.FirstOrDefault(u => u.GamePlayer.Player.UserName == winnerUserName);

            //check if there is justpicked tile from winner to determine if its selfpick
            var isSelfPick = tiles.Any(t => t.Status == TileStatus.UserJustPicked);

            List<RoundTile> totalTiles = tiles.ToList();

            if (!isSelfPick)
            {
                //then there gotta be board active tile
                var boardActiveTile = round.RoundTiles.FirstOrDefault(t => t.Status == TileStatus.BoardActive && t.Owner == DefaultValue.board);
                if (boardActiveTile == null)
                    throw new Exception("somehow can't find board tile active when its not self pick and user can win");

                totalTiles.Add(boardActiveTile);
            }

            //if this is user's wind
            var userCurrentWind = winner.Wind.ToTileValue();
            var userWind = totalTiles.Where(t => t.Tile.TileValue == userCurrentWind);
            if (userWind.Count() >= 3)
                extraPoints.Add(ExtraPoint.SeatWind);

            //if this is prevailing wind
            var currentWind = round.Wind.ToTileValue();
            var prevailingWinds = totalTiles.Where(t => t.Tile.TileValue == currentWind);
            if (prevailingWinds.Count() >= 3)
                extraPoints.Add(ExtraPoint.PrevailingWind);

            if (_successor != null)
                return _successor.HandleRequest(round, winnerUserName, extraPoints);
            else
                return extraPoints;
        }
    }
}
