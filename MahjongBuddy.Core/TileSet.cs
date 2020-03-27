using System;
using System.Collections.Generic;

namespace MahjongBuddy.Core
{
    public class TileSet
    {
        public Guid Id { get; set; }
        public virtual ICollection<GameTile> GameTiles { get; set; }
        public TileSetType TileSetType { get; set; }
        public TileSetGroup TileSetGroup { get; set; }
        public bool IsRevealed { get; set; }
    }
}
