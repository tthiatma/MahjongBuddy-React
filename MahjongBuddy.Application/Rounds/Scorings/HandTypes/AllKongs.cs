using MahjongBuddy.Core;
using System.Collections.Generic;
using System.Linq;

namespace MahjongBuddy.Application.Rounds.Scorings.HandTypes
{
    class AllKongs : FindHandType
    {
        public override List<HandType> HandleRequest(IEnumerable<RoundTile> tiles, List<HandType> handTypes)
        {            
            var allkongs = tiles.Where(rt => rt.TileSetGroup == TileSetGroup.Kong);

            if (allkongs.Count() == 16)
            {
                handTypes.Add(HandType.AllKongs);
            }

            if (_successor != null)
                return _successor.HandleRequest(tiles, handTypes);
            else
                return handTypes;
        }
    }
}
