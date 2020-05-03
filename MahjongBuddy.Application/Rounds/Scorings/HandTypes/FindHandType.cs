using MahjongBuddy.Core;
using System.Collections.Generic;

namespace MahjongBuddy.Application.Rounds.Scorings
{
    abstract class FindHandType
    {
        protected FindHandType _successor;
        protected IEnumerable<RoundTile> _tiles;
        public FindHandType(IEnumerable<RoundTile> tiles)
        {
            _tiles = tiles;
        }
        public void SetSuccessor(FindHandType successor)
        {
            _successor = successor;
        }

        public abstract HandType HandleRequest(IEnumerable<RoundTile> tiles);
    }
}
