namespace MahjongBuddy.Core
{
    public class Circle1 : Tile
    {
        public Circle1()
        {
            TileType = TileType.Circle;
            TileValue = TileValue.One;
            Title = "RoundOne";
            Image = "/assets/tiles/64px/pin/pin1.png";
            ImageSmall = "/assets/tiles/50px/pin/pin1.png";
        }
    }
}