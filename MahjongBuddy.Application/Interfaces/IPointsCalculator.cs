using MahjongBuddy.Application.Rounds.Scorings;
using MahjongBuddy.Core;
using MahjongBuddy.Core.Enums;
using System.Collections.Generic;

namespace MahjongBuddy.Application.Interfaces
{
    public interface IPointsCalculator
    {
        public Dictionary<HandType, int> HandTypeLookup { get;}

        public Dictionary<ExtraPoint, int> ExtraPointLookup { get;}

        public HandWorth Calculate(Round round, string winnerUserName);
    }
}
