using System.Collections.Generic;

namespace MahjongBuddy.Core
{
    public class Player
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsPlaying { get; set; }
        public bool CanPickTile { get; set; }
        public bool CanThrowTile { get; set; }
        public bool CanDoNoFlower { get; set; }
        public int CurrentPoint { get; set; }
        public List<TileSet> TileSets { get; set; }
    }
}
