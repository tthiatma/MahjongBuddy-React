namespace MahjongBuddy.Core
{
    public class Circle7 : Tile
    {
        public Circle7()
        {
            Id = 17;
            TileType = TileType.Circle;
            TileValue = TileValue.Seven;
            Title = "RoundSeven";
            Image = "/assets/tiles/64px/pin/pin7.png";
            ImageSmall = "/assets/tiles/50px/pin/pin7.png";
        }
    }
}