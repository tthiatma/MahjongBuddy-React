using MahjongBuddy.Application.Helpers;
using MahjongBuddy.Core;
using System.Collections.Generic;

namespace MahjongBuddy.Application.Rounds.Scorings.HandTypes
{
    class LegitSet : FindHandType
    {
        public override List<HandType> HandleRequest(IEnumerable<RoundTile> tiles, List<HandType> handTypes)
        {
            if (tiles == null)
                return handTypes;

            var result = RoundTileHelper.DetermineHandCanWin(tiles);

            if (result == HandType.Triplets || result == HandType.Straight || result == HandType.Chicken)
            {
                if (_successor != null)
                {
                    return _successor.HandleRequest(tiles, handTypes);
                }
                else 
                {
                    return handTypes;
                }
            }
            else
            {
                return handTypes;
            }
        }
    }
}
