using MahjongBuddy.Core;
using System.Collections.Generic;

namespace MahjongBuddy.Application.Rounds.Scorings
{
    abstract class FindHandType
    {
        protected FindHandType _successor;
        public void SetSuccessor(FindHandType successor)
        {
            _successor = successor;
        }

        public abstract List<HandType> HandleRequest(IEnumerable<RoundTile> tiles, List<HandType> result);
    }
}
