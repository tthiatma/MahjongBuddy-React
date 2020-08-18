using MahjongBuddy.Core;
using System.Collections.Generic;

namespace MahjongBuddy.Application.Helpers
{
    public class SortAliveTiles : IComparer<RoundTile>
    {
        public int Compare(RoundTile a, RoundTile b)
        {
            if (a.Status > b.Status) return -1;
            if (a.Status < b.Status) return 1;
            if (a.TileSetGroupIndex < b.TileSetGroupIndex) return -1;
            if (a.TileSetGroupIndex > b.TileSetGroupIndex) return 1;
            if (a.Tile.TileType > b.Tile.TileType) return -1;
            if (a.Tile.TileType < b.Tile.TileType) return 1;
            if (a.Tile.TileValue > b.Tile.TileValue) return 1;
            if (a.Tile.TileValue < b.Tile.TileValue) return -1;
            return 0;
        }
    }
}
