namespace MahjongBuddy.Core
{
    public class Stick7 : Tile
    {
        public Stick7()
        {
            Type = TileType.Stick;
            Value = TileValue.Seven;
            Name = "StickSeven";
            Image = "/assets/tiles/64px/bamboo/bamboo7.png";
            ImageSmall = "/assets/tiles/50px/bamboo/bamboo7.png";
        }
    }
}