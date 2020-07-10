namespace MahjongBuddy.Core
{
    public class Circle4 : Tile
    {
        public Circle4()
        {
            TileType = TileType.Circle;
            TileValue = TileValue.Four;
            Title = "RoundFour";
            Image = "/assets/tiles/64px/pin/pin4.png";
            ImageSmall = "/assets/tiles/50px/pin/pin4.png";
        }
    }
}