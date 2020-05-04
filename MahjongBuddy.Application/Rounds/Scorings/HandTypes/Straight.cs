using MahjongBuddy.Application.Helpers;
using MahjongBuddy.Core;
using System.Collections.Generic;

namespace MahjongBuddy.Application.Rounds.Scorings.HandTypes
{
    class Straight : FindHandType
    {
        public override List<HandType> HandleRequest(IEnumerable<RoundTile> tiles, List<HandType> handTypes)
        {
            if (tiles == null)
                return handTypes;

            var result = RoundTileHelper.DetermineHandCanWin(tiles);

            if (result == HandType.Straight)
            {
                handTypes.Add(HandType.Straight);
                return handTypes;
            }
            else
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
        }
    }
}
