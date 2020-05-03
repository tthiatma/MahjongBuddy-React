using MahjongBuddy.Core;
using System;
using System.Collections.Generic;
using System.Text;

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
            _initial.SetSuccessor(thirteenOrphans);
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
