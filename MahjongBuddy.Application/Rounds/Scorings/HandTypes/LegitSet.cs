using MahjongBuddy.Application.Helpers;
using MahjongBuddy.Core;
using System.Collections.Generic;

namespace MahjongBuddy.Application.Rounds.Scorings.HandTypes
{
    class LegitSet : FindHandType
    {
        public LegitSet(IEnumerable<RoundTile> tiles) : base(tiles){}
        public override HandType HandleRequest(IEnumerable<RoundTile> tiles)
        {
            if (tiles == null)
                return HandType.None;

            //check al pairs      
            var result = RoundTileHelper.DetermineHandCanWin(tiles);

            if (result == HandType.Triplets || result == HandType.Straight || result == HandType.Chicken)
            {
                if (_successor != null)
                {
                    return _successor.HandleRequest(tiles);
                }
                else 
                {
                    return HandType.None;
                }
            }
            else
            {
                return HandType.None;
            }
        }
    }
}
