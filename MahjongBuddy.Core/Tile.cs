namespace MahjongBuddy.Core
{
    public class Tile
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public TileType TileType { get; set; }
        public TileValue TileValue { get; set; }
        public string Image { get; set; }
        public string ImageSmall { get; set; }
    }
}
