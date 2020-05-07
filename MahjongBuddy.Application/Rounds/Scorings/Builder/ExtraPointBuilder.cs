using MahjongBuddy.Application.Rounds.Scorings.ExtraPoints;
using MahjongBuddy.Core;
using MahjongBuddy.Core.Enums;
using System.Collections.Generic;
namespace MahjongBuddy.Application.Rounds.Scorings.Builder
{
    public class ExtraPointBuilder
    {
        readonly FindExtraPoint _initial;
        List<ExtraPoint> _extraPoints;

        public ExtraPointBuilder()
        {
            _extraPoints = new List<ExtraPoint>();
            _initial = new ExtraConcealed();
            FindExtraPoint dragon = new ExtraDragon();
            FindExtraPoint flower = new ExtraFlower();
            FindExtraPoint lastTile = new ExtraLastTile();
            FindExtraPoint selfPick = new ExtraSelfPick();
            FindExtraPoint wind = new ExtraWind();

            _initial.SetSuccessor(dragon);
            dragon.SetSuccessor(flower);
            flower.SetSuccessor(lastTile);
            lastTile.SetSuccessor(selfPick);
            selfPick.SetSuccessor(wind);
        }

        public List<ExtraPoint> GetExtraPoint(Round round, string winnerUserName)
        {
            if (round != null && !string.IsNullOrEmpty(winnerUserName))
                return _initial.HandleRequest(round, winnerUserName, _extraPoints);
            else
                return _extraPoints;
        }
    }
}
