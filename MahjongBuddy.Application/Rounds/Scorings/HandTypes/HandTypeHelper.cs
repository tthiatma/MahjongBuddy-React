using MahjongBuddy.Application.Rounds.Scorings.HandTypes;
using MahjongBuddy.Core;
using System.Collections.Generic;

namespace MahjongBuddy.Application.Rounds.Scorings
{
    public class HandTypeHelper
    {
        readonly IEnumerable<RoundTile> _tiles;
        readonly FindHandType _initial;
        public HandTypeHelper(IEnumerable<RoundTile> tiles)
        {
            _tiles = tiles;
            _initial = new SevenPairs();
            FindHandType thirteenOrphans = new ThirteenOrphans();
            FindHandType legitSet = new LegitSet();
            FindHandType triplets = new Triplets();
            FindHandType straight = new Straight();

            _initial.SetSuccessor(thirteenOrphans);
            thirteenOrphans.SetSuccessor(legitSet);
            legitSet.SetSuccessor(triplets);
            triplets.SetSuccessor(straight);
        }

        public HandType GetHandType()
        {
            if (_tiles != null)
                return _initial.HandleRequest(_tiles);
            else
                return HandType.None;
        }
    }
}
