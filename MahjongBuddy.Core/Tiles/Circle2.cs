namespace MahjongBuddy.Core
{
    public class Circle2 : Tile
    {
        public Circle2()
        {
            TileType = TileType.Circle;
            TileValue = TileValue.Two;
            Title = "RoundTwo";
            Image = "/assets/tiles/64px/pin/pin2.png";
            ImageSmall = "/assets/tiles/50px/pin/pin2.png";
        }
    }
}