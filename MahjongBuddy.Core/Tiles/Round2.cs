namespace MahjongBuddy.Core
{
    public class Round2 : Tile
    {
        public Round2()
        {
            Id = 12;
            TileType = TileType.Round;
            TileValue = TileValue.Two;
            Title = "RoundTwo";
            Image = "/assets/tiles/64px/pin/pin2.png";
            ImageSmall = "/assets/tiles/50px/pin/pin2.png";
        }
    }
}