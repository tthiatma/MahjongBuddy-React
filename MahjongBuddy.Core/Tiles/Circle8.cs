namespace MahjongBuddy.Core
{
    public class Circle8 : Tile
    {
        public Circle8()
        {
            TileType = TileType.Circle;
            TileValue = TileValue.Eight;
            Title = "RoundEight";
            Image = "/assets/tiles/64px/pin/pin8.png";
            ImageSmall = "/assets/tiles/50px/pin/pin8.png";
        }
    }
}