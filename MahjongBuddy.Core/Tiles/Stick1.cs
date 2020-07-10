namespace MahjongBuddy.Core
{
    public class Stick1 : Tile
    {
        public Stick1()
        {
            TileType = TileType.Stick;
            TileValue = TileValue.One;
            Title = "StickOne";
            Image = "/assets/tiles/64px/bamboo/bamboo1.png";
            ImageSmall = "/assets/tiles/50px/bamboo/bamboo1.png";
        }
    }
}