﻿using MahjongBuddy.Core;
using System.Collections.Generic;
using System.Linq;

namespace MahjongBuddy.Application.Rounds.Scorings
{
    class SevenPairs : FindHandType
    {
        public override List<HandType> HandleRequest(IEnumerable<RoundTile> tiles, List<HandType> handTypes)
        {
            //2-2-2-2-2-2-2 all tiles consist of pairs it can't be flowers obviously
            if (tiles == null)
                return handTypes;

            //check al pairs      
            bool isAllPair = true;
            foreach (var t in tiles)
            {
                var pairTiles = tiles.ToList().Where(tt => tt.Tile.TileType == t.Tile.TileType && tt.Tile.TileValue== t.Tile.TileValue);
                if (pairTiles != null)
                {
                    if (pairTiles.Count() == 1 || pairTiles.Count() == 3)
                    {
                        isAllPair = false;
                        break;
                    }
                }
            }

            if (isAllPair)
            {
                handTypes.Add(HandType.SevenPairs);
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
