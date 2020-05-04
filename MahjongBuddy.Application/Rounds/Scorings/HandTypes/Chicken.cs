using MahjongBuddy.Application.Helpers;
using MahjongBuddy.Core;
using System.Collections.Generic;

namespace MahjongBuddy.Application.Rounds.Scorings.HandTypes
{
    class Chicken : FindHandType
    {
        public override HandType HandleRequest(IEnumerable<RoundTile> tiles)
        {
            if (tiles == null)
                return HandType.None;

            var result = RoundTileHelper.DetermineHandCanWin(tiles);

            if (result == HandType.Chicken)
                return HandType.Chicken;
            else
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
        }
    }
}
