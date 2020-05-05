using MahjongBuddy.Core;
using System.Collections.Generic;

namespace MahjongBuddy.Application.Rounds.Scorings.ExtraPoints
{
    abstract class FindExtraPoints
    {
        protected FindExtraPoints _successor;
        public void SetSuccessor(FindExtraPoints successor)
        {
            _successor = successor;
        }

        public abstract HandType HandleRequest(IEnumerable<RoundTile> tiles, Round round);
    }
}
