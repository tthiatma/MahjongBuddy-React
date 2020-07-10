namespace MahjongBuddy.Core
{
    public class Stick6 : Tile
    {
        public Stick6()
        {
            TileType = TileType.Stick;
            TileValue = TileValue.Six;
            Title = "StickSix";
            Image = "/assets/tiles/64px/bamboo/bamboo6.png";
            ImageSmall = "/assets/tiles/50px/bamboo/bamboo6.png";
        }
    }
}