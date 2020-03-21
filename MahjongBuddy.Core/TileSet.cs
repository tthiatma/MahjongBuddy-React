using System.Collections.Generic;

namespace MahjongBuddy.Core
{
    public class TileSet
    {
        public IEnumerable<Tile> Tiles { get; set; }
        public TileSetType TileSetType { get; set; }
        public TileSetGroup TileSetGroup { get; set; }
        public bool isRevealed { get; set; }
    }
}
