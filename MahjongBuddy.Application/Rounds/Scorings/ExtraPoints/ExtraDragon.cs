﻿using MahjongBuddy.Core;
using MahjongBuddy.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MahjongBuddy.Application.Rounds.Scorings.ExtraPoints
{
    class ExtraDragon : FindExtraPoint
    {
        public override List<ExtraPoint> HandleRequest(Round round, string winnerUserName, List<ExtraPoint> extraPoints)
        {
            var tiles = round.RoundTiles.Where(t => t.Owner == winnerUserName);

            //check if there is justpicked tile from winner to determine if its selfpick
            var isSelfPick = tiles.Any(t => t.Status == TileStatus.UserJustPicked);

            List<RoundTile> totalTiles = tiles.ToList();

            if (!isSelfPick)
            {
                //then there gotta be board active tile
                var boardActiveTile = round.RoundTiles.FirstOrDefault(t => t.Status == TileStatus.BoardActive && t.Owner == DefaultValue.board);
                if(boardActiveTile == null)
                    throw new Exception("somehow can't find board tile active when its not self pick and user can win");

                totalTiles.Add(boardActiveTile);
            }

            var redDragonTiles = totalTiles.Where(t => t.Tile.TileValue == TileValue.DragonRed);
            if (redDragonTiles.Count() >= 3)
                extraPoints.Add(ExtraPoint.RedDragon);

            var greenDragonTiles = totalTiles.Where(t => t.Tile.TileValue == TileValue.DragonGreen);
            if (greenDragonTiles.Count() >= 3)
                extraPoints.Add(ExtraPoint.GreenDragon);

            var whiteDragonTiles = totalTiles.Where(t => t.Tile.TileValue == TileValue.DragonWhite);
            if (whiteDragonTiles.Count() >= 3)
                extraPoints.Add(ExtraPoint.WhiteDragon);

            if (_successor != null)
                return _successor.HandleRequest(round, winnerUserName, extraPoints);
            else
                return extraPoints;
        }
    }
}
