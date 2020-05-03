using MahjongBuddy.Core;
using System.Collections.Generic;

namespace MahjongBuddy.Application.Rounds.Scorings.ExtraPoints
{
    abstract class FindExtraPoints
    {
        protected FindExtraPoints _successor;
        protected IEnumerable<RoundTile> _tiles;
        protected Round _round;

        public FindExtraPoints(IEnumerable<RoundTile> tiles, Round round)
        {
            _tiles = tiles;
            _round = round;
        }
        public void SetSuccessor(FindExtraPoints successor)
        {
            _successor = successor;
        }

        public abstract HandType HandleRequest(IEnumerable<RoundTile> tiles, Round round);
    }
}
