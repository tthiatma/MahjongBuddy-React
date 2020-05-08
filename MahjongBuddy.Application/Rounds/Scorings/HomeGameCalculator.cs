using MahjongBuddy.Application.Interfaces;
using MahjongBuddy.Application.Rounds.Scorings.Builder;
using MahjongBuddy.Core;
using MahjongBuddy.Core.Enums;
using System.Collections.Generic;
using System.Linq;

namespace MahjongBuddy.Application.Rounds.Scorings
{
    public class HomeGameCalculator : IPointsCalculator
    {
        private readonly ExtraPointBuilder _pointBuider;
        private readonly HandTypeBuilder _handBuilder;
        private readonly Dictionary<HandType, int> _handTypeLookup;
        private readonly Dictionary<ExtraPoint, int> _extraPointLookup;

        public HomeGameCalculator(ExtraPointBuilder pointBuilder, HandTypeBuilder handBuilder)
        {
            _pointBuider = pointBuilder;
            _handBuilder = handBuilder;
            _handTypeLookup = new Dictionary<HandType, int>()
            {
                { HandType.AllOneSuit, 7 },
                { HandType.BigDragon, 10 },
                { HandType.BigFourWind, 10 },
                { HandType.Chicken, 0 },
                { HandType.MixedOneSuit, 3 },
                { HandType.SevenPairs, 7 },
                { HandType.SmallDragon, 5 },
                { HandType.SmallFourWind, 10 },
                { HandType.Straight, 1 },
                { HandType.ThirteenOrphans, 10 },
                { HandType.Triplets, 3 },
                { HandType.None, -10 },
            };
            _extraPointLookup = new Dictionary<ExtraPoint, int>()
            {
                { ExtraPoint.AllFourFlowerSameType, 1 },
                { ExtraPoint.ConcealedHand, 1 },
                { ExtraPoint.GreenDragon, 1 },
                { ExtraPoint.NoFlower, 1 },
                { ExtraPoint.None, 0 },
                { ExtraPoint.NumericFlower, 1 },
                { ExtraPoint.PrevailingWind, 1 },
                { ExtraPoint.RedDragon, 1 },
                { ExtraPoint.RomanFlower, 1 },
                { ExtraPoint.SeatWind, 1 },
                { ExtraPoint.SelfPick, 1 },
                { ExtraPoint.WhiteDragon, 1 },
                { ExtraPoint.WinOnLastTile, 1 },
            };

        }
        public HandWorth Calculate(Round round, string winnerUserName)
        {
            HandWorth ret = new HandWorth();
            int totalPoints = 0;
            var handTypes = _handBuilder.GetHandType(round, winnerUserName);
            if(handTypes.Count() > 0)
            {
                handTypes.ForEach(tp => totalPoints += _handTypeLookup[tp]);

                var extraPoints = _pointBuider.GetExtraPoint(round, winnerUserName);
                extraPoints.ForEach(ep => totalPoints += _extraPointLookup[ep]);

                ret.HandTypes = handTypes;
                ret.ExtraPoints = extraPoints;
                ret.Points = totalPoints;
            }
            return ret;
        }
    }
}
