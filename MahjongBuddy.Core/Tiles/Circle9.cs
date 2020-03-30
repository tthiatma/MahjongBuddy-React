namespace MahjongBuddy.Core
{
    public class Circle9 : Tile
    {
        public Circle9()
        {
            Id = 19;
            TileType = TileType.Circle;
            TileValue = TileValue.Nine;
            Title = "RoundNine";
            Image = "/assets/tiles/64px/pin/pin9.png";
            ImageSmall = "/assets/tiles/50px/pin/pin9.png";
        }
    }
}