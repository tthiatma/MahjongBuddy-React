namespace MahjongBuddy.Core
{
    public class Stick7 : Tile
    {
        public Stick7()
        {
            TileType = TileType.Stick;
            TileValue = TileValue.Seven;
            Title = "StickSeven";
            Image = "/assets/tiles/64px/bamboo/bamboo7.png";
            ImageSmall = "/assets/tiles/50px/bamboo/bamboo7.png";
        }
    }
}