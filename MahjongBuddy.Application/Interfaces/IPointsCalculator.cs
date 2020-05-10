using MahjongBuddy.Application.Rounds.Scorings;
using MahjongBuddy.Core;

namespace MahjongBuddy.Application.Interfaces
{
    public interface IPointsCalculator
    {

        public HandWorth Calculate(Round round, string winnerUserName);
    }
}
