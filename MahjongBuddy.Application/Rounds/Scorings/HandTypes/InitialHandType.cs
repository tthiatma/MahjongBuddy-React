using MahjongBuddy.Core;
using System;
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

            if (tiles.Count() < 14)
                throw new Exception("invalid count of tiles");

            //remove flower tiles if exist because handtype ignore flower
            var tilesList = tiles.ToList();
            var userFlowerTiles = tiles.Where(t => t.Tile.TileType == TileType.Flower);
            foreach (var item in userFlowerTiles)
            {
                tilesList.Remove(item);
            }

            if (_successor != null)
                return _successor.HandleRequest(tilesList, handTypes);
            else
                return handTypes;
        }
    }
}
