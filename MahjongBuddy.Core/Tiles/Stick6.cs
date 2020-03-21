namespace MahjongBuddy.Core
{
    public class Stick6 : Tile
    {
        public Stick6()
        {
            Type = TileType.Stick;
            Value = TileValue.Six;
            Name = "StickSix";
            Image = "/assets/tiles/64px/bamboo/bamboo6.png";
            ImageSmall = "/assets/tiles/50px/bamboo/bamboo6.png";
        }
    }
}