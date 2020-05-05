using MahjongBuddy.Core;
using MahjongBuddy.Core.Enums;
using System.Collections.Generic;

namespace MahjongBuddy.Application.Rounds.Scorings.ExtraPoints
{
    abstract class FindExtraPoint
    {
        protected FindExtraPoint _successor;
        public void SetSuccessor(FindExtraPoint successor)
        {
            _successor = successor;
        }

        public abstract List<ExtraPoint> HandleRequest(IEnumerable<RoundTile> tiles, Round round, List<ExtraPoint> extraPoints);
    }
}
