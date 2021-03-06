﻿using MahjongBuddy.Application.Rounds.Scorings.ExtraPoints;
using MahjongBuddy.Core;
using MahjongBuddy.Core.Enums;
using System.Collections.Generic;
namespace MahjongBuddy.Application.Rounds.Scorings.Builder
{
    public class ExtraPointBuilder
    {
        readonly FindExtraPoint _initial;

        public ExtraPointBuilder()
        {
            _initial = new ExtraConcealed();
            FindExtraPoint dragon = new ExtraDragon();
            FindExtraPoint flower = new ExtraFlower();
            FindExtraPoint selfPick = new ExtraSelfPick();
            FindExtraPoint lastTile = new ExtraLastTile();
            FindExtraPoint wind = new ExtraWind();

            _initial.SetSuccessor(dragon);
            dragon.SetSuccessor(flower);
            flower.SetSuccessor(selfPick);
            //important for last tile to be after selfpick because last tile check for selfpick 
            selfPick.SetSuccessor(lastTile);
            lastTile.SetSuccessor(wind);
        }

        public List<ExtraPoint> GetExtraPoint(Round round, string winnerUserName)
        {
            List<ExtraPoint> extraPoints = new List<ExtraPoint>();
            if (round != null && !string.IsNullOrEmpty(winnerUserName))
                return _initial.HandleRequest(round, winnerUserName, extraPoints);
            else
                return extraPoints;
        }
    }
}
