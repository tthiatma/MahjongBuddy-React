namespace MahjongBuddy.Core
{
    public class Circle6 : Tile
    {
        public Circle6()
        {
            TileType = TileType.Circle;
            TileValue = TileValue.Six;
            Title = "RoundSix";
            Image = "/assets/tiles/64px/pin/pin6.png";
            ImageSmall = "/assets/tiles/50px/pin/pin6.png";
        }
    }
}