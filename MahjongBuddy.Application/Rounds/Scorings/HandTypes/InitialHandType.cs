using MahjongBuddy.Core;
using System.Collections.Generic;
using System.Linq;
namespace MahjongBuddy.Application.Rounds.Scorings.HandTypes
{
    class InitialHandType : FindHandType
    {
        public override List<HandType> HandleRequest(IEnumerable<RoundTile> tiles, List<HandType> handTypes)
        {
            if (tiles == null)
                return handTypes;

            //remove flower tiles if exist because handtype ignore flower
            tiles = tiles.Except(tiles.Where(t => t.Tile.TileType == TileType.Flower));

            if (_successor != null)
                return _successor.HandleRequest(tiles, handTypes);
            else
                return handTypes;
        }
    }
}
