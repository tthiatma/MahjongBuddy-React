using MahjongBuddy.Core;
using System.Collections.Generic;
using System.Linq;

namespace MahjongBuddy.Application.Rounds.Scorings.HandTypes
{
    class HiddenTreasure : FindHandType
    {
        //hidden treasure is when all concealed and all pong tiles and self picked
        public override List<HandType> HandleRequest(IEnumerable<RoundTile> tiles, List<HandType> handTypes)
        {
            //short circuit if there is any group set
            var hasGroupSet = tiles.Any(rt => rt.TileSetGroupIndex > 0);
            if(hasGroupSet)
            {
                if (_successor != null)
                    return _successor.HandleRequest(tiles, handTypes);
                else
                    return handTypes;
            }

            //if it gets all the way to here, that means it pass "LegitSet" hand type and we already know if it has triplet hand type
            var isSelfPicked = tiles.Any(t => t.Status == TileStatus.UserJustPicked);

            if(handTypes.Contains(HandType.Triplets) && isSelfPicked)
            {
                handTypes.Add(HandType.HiddenTreasure);
            }

            if (_successor != null)
                return _successor.HandleRequest(tiles, handTypes);
            else
                return handTypes;
        }
    }
}
