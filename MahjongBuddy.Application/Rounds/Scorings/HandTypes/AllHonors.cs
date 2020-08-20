using MahjongBuddy.Core;
using System.Collections.Generic;
using System.Linq;

namespace MahjongBuddy.Application.Rounds.Scorings.HandTypes
{
    class AllHonors : FindHandType
    {
        public override List<HandType> HandleRequest(IEnumerable<RoundTile> tiles, List<HandType> handTypes)
        {
            var nonHonors = tiles.Where(rt => rt.Tile.TileType == TileType.Circle || rt.Tile.TileType == TileType.Stick || rt.Tile.TileType == TileType.Money);

            if (nonHonors.Count() == 0)
            {
                handTypes.Add(HandType.AllHonors);
            }

            if (_successor != null)
                return _successor.HandleRequest(tiles, handTypes);
            else
                return handTypes;
        }
    }
}
