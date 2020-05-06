using MahjongBuddy.Application.Rounds.Scorings.ExtraPoints;
using MahjongBuddy.Core;
using MahjongBuddy.Core.Enums;
using System.Collections.Generic;
namespace MahjongBuddy.Application.Rounds.Scorings.Builder
{
    public class ExtraPointBuilder
    {
        readonly Round _round;
        readonly FindExtraPoint _initial;
        readonly string _winnerUserName;
        List<ExtraPoint> _extraPoints;

        public ExtraPointBuilder(Round round, string winnerUserName)
        {
            _extraPoints = new List<ExtraPoint>();
            _round = round;
            _winnerUserName = winnerUserName;
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

        public List<ExtraPoint> GetExtraPoint()
        {
            if (_round != null && !string.IsNullOrEmpty(_winnerUserName))
                return _initial.HandleRequest(_round, _winnerUserName, _extraPoints);
            else
                return _extraPoints;
        }
    }
}
