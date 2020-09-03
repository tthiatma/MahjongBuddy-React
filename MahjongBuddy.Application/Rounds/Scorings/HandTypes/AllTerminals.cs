using MahjongBuddy.Core;
using System.Collections.Generic;
using System.Linq;

namespace MahjongBuddy.Application.Rounds.Scorings.HandTypes
{
    class AllTerminals : FindHandType
    {
        public override List<HandType> HandleRequest(IEnumerable<RoundTile> tiles, List<HandType> handTypes)
        {
            //short circuit if there is chow tiles
            var chowTiles = tiles.Where(rt => rt.TileSetGroup == TileSetGroup.Chow);
            var dragonOrWindTiles = tiles.Where(rt => rt.Tile.TileType == TileType.Wind || rt.Tile.TileType == TileType.Dragon);
            if(chowTiles.Count() > 0)
            {
                if (_successor != null)
                    return _successor.HandleRequest(tiles, handTypes);
                else
                    return handTypes;
            }

            var commonTileType = tiles.Where(rt => rt.Tile.TileType == TileType.Circle || rt.Tile.TileType == TileType.Stick || rt.Tile.TileType == TileType.Money);

            var notOneOrNine = commonTileType.Where(
                rt => rt.Tile.TileValue == TileValue.Two ||
                rt.Tile.TileValue == TileValue.Three ||
                rt.Tile.TileValue == TileValue.Four ||
                rt.Tile.TileValue == TileValue.Five ||
                rt.Tile.TileValue == TileValue.Six ||
                rt.Tile.TileValue == TileValue.Seven ||
                rt.Tile.TileValue == TileValue.Eight
            );

            if(notOneOrNine.Count() == 0)
            {
                if(dragonOrWindTiles.Count() > 0)
                    handTypes.Add(HandType.MixedAllTerminal);
                else
                    handTypes.Add(HandType.AllTerminals);
            }

            if (_successor != null)
                return _successor.HandleRequest(tiles, handTypes);
            else
                return handTypes;
        }
    }
}
