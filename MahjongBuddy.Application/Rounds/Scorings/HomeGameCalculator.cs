using MahjongBuddy.Application.Interfaces;
using MahjongBuddy.Application.Rounds.Scorings.Builder;
using MahjongBuddy.Core;
using MahjongBuddy.Core.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace MahjongBuddy.Application.Rounds.Scorings
{
    public class HomeGameCalculator : IPointsCalculator
    {
        private readonly ExtraPointBuilder _pointBuider;
        private readonly HandTypeBuilder _handBuilder;

        public HomeGameCalculator(ExtraPointBuilder pointBuilder, HandTypeBuilder handBuilder)
        {
            _pointBuider = pointBuilder;
            _handBuilder = handBuilder;
        }
        public HandWorth Calculate(Round round, string winnerUserName)
        {
            HandWorth ret = new HandWorth();
            ret.HandTypes = _handBuilder.GetHandType(round, winnerUserName);
            ret.ExtraPoints = _pointBuider.GetExtraPoint(round, winnerUserName);
            return ret;
        }
    }
}
