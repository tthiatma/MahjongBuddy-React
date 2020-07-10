namespace MahjongBuddy.Core
{
    public class Circle5 : Tile
    {
        public Circle5()
        {
            TileType = TileType.Circle;
            TileValue = TileValue.Five;
            Title = "RoundFive";
            Image = "/assets/tiles/64px/pin/pin5.png";
            ImageSmall = "/assets/tiles/50px/pin/pin5.png";
        }
    }
}