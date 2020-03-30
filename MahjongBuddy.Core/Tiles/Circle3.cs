namespace MahjongBuddy.Core
{
    public class Circle3 : Tile
    {
        public Circle3()
        {
            Id = 13;
            TileType = TileType.Circle;
            TileValue = TileValue.Three;
            Title = "RoundThree";
            Image = "/assets/tiles/64px/pin/pin3.png";
            ImageSmall = "/assets/tiles/50px/pin/pin3.png";
        }
    }
}