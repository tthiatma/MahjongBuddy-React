using System;
using System.Collections.Generic;

namespace MahjongBuddy.Core
{
    public class WinningTileSet
    {
        public TileSet Eye { get; set; }
        public TileSet[] TileSets { get; set; }
        public List<Tile> Flowers { get; set; }
        public List<HandWorth> Hands { get; set; }
        public Dictionary<string, int> WinningTypes { get; set; }
        public int TotalPoints { get; set; }

        public WinningTileSet()
        {
            TileSets = new TileSet[4];
            Flowers = new List<Tile>();
            Eye = new TileSet();
            Eye.TileSetGroup = TileSetGroup.Eye;
            Hands = new List<HandWorth>();
            WinningTypes = new Dictionary<String, int>();
        }
    }
}
