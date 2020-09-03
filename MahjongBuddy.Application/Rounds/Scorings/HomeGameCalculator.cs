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
        public Dictionary<HandType, int> HandTypeLookup { get; }
        public Dictionary<ExtraPoint, int> ExtraPointLookup { get; }

        public HomeGameCalculator(ExtraPointBuilder pointBuilder, HandTypeBuilder handBuilder)
        {
            _pointBuider = pointBuilder;
            _handBuilder = handBuilder;
            HandTypeLookup = new Dictionary<HandType, int>()
            {
                { HandType.None, 0 },
                { HandType.AllOneSuit, 7 },
                { HandType.BigDragon, 10 },
                { HandType.BigFourWind, 13 },
                { HandType.Chicken, 0 },
                { HandType.MixedOneSuit, 3 },
                { HandType.SevenPairs, 6 },
                { HandType.SmallDragon, 5 },
                { HandType.SmallFourWind, 10 },
                { HandType.Straight, 1 },
                { HandType.ThirteenOrphans, 13 },
                { HandType.Triplets, 3 },
                { HandType.HiddenTreasure, 13},
                { HandType.AllTerminals, 10},
                { HandType.MixedAllTerminal, 7},
                { HandType.AllKongs, 13 },
                { HandType.AllHonors, 10 },
            };
            ExtraPointLookup = new Dictionary<ExtraPoint, int>()
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
                handTypes.ForEach(tp => totalPoints += HandTypeLookup[tp]);

                var extraPoints = _pointBuider.GetExtraPoint(round, winnerUserName);

                //if handtypes has seven pairs then take off concealed hand extrapoint
                if(handTypes.Contains(HandType.SevenPairs) || handTypes.Contains(HandType.ThirteenOrphans))
                {
                    if (extraPoints.Contains(ExtraPoint.ConcealedHand)) extraPoints.Remove(ExtraPoint.ConcealedHand);
                }

                //if handtypes has small dragon or big dragon then take off dragon extrapoint 
                if (handTypes.Contains(HandType.SmallDragon) || handTypes.Contains(HandType.BigDragon))
                {
                    if (extraPoints.Contains(ExtraPoint.GreenDragon)) extraPoints.Remove(ExtraPoint.GreenDragon);
                    if (extraPoints.Contains(ExtraPoint.RedDragon)) extraPoints.Remove(ExtraPoint.RedDragon);
                    if (extraPoints.Contains(ExtraPoint.WhiteDragon)) extraPoints.Remove(ExtraPoint.WhiteDragon);
                }

                //if handtypes has small wind or big wind then take off wind extrapoint
                if (handTypes.Contains(HandType.SmallFourWind) || handTypes.Contains(HandType.BigFourWind))
                {
                    if (extraPoints.Contains(ExtraPoint.PrevailingWind)) extraPoints.Remove(ExtraPoint.PrevailingWind);
                    if (extraPoints.Contains(ExtraPoint.SeatWind)) extraPoints.Remove(ExtraPoint.SeatWind);
                }

                //if handtypes contain below, then triplets wont count anymore
                if (handTypes.Contains(HandType.HiddenTreasure) || 
                    handTypes.Contains(HandType.AllTerminals) ||
                    handTypes.Contains(HandType.MixedAllTerminal) ||
                    handTypes.Contains(HandType.AllHonors) ||
                    handTypes.Contains(HandType.AllKongs) 
                    )
                {
                    if (handTypes.Contains(HandType.Triplets)) handTypes.Remove(HandType.Triplets);
                }

                extraPoints.ForEach(ep => totalPoints += ExtraPointLookup[ep]);

                ret.HandTypes = handTypes;
                ret.ExtraPoints = extraPoints;
                ret.Points = totalPoints;
            }
            else
            {
                ret = null;
            }
            return ret;
        }
    }
}
