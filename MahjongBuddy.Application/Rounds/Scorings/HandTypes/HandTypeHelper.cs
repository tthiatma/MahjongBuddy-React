using MahjongBuddy.Application.Rounds.Scorings.HandTypes;
using MahjongBuddy.Core;
using System.Collections.Generic;

namespace MahjongBuddy.Application.Rounds.Scorings
{
    public class HandTypeHelper
    {
        readonly IEnumerable<RoundTile> _tiles;
        readonly FindHandType _initial;
        List<HandType> _handtypes;
        public HandTypeHelper(IEnumerable<RoundTile> tiles)
        {
            _tiles = tiles;
            _handtypes = new List<HandType>();
            _initial = new SevenPairs();
            FindHandType thirteenOrphans = new ThirteenOrphans();
            FindHandType legitSet = new LegitSet();
            FindHandType mixedOneSuit = new OneSuit();

            _initial.SetSuccessor(thirteenOrphans);
            thirteenOrphans.SetSuccessor(legitSet);
            legitSet.SetSuccessor(mixedOneSuit);
        }

        public List<HandType> GetHandType()
        {
            if (_tiles != null)
                return _initial.HandleRequest(_tiles, _handtypes);
            else
                return _handtypes;
        }
    }
}
