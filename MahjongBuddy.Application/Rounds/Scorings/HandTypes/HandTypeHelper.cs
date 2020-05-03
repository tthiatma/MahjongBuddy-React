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
            _initial = new SevenPairs(tiles);
            FindHandType thirteenOrphans = new ThirteenOrphans(tiles);
            FindHandType legitSet = new LegitSet(tiles);
            FindHandType triplets = new Triplets(tiles);
            FindHandType straight = new Straight(tiles);

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
