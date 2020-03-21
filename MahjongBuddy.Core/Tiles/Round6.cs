namespace MahjongBuddy.Core
{
    public class Round6 : Tile
    {
        public Round6()
        {
            Id = 16;
            TileType = TileType.Round;
            TileValue = TileValue.Six;
            Title = "RoundSix";
            Image = "/assets/tiles/64px/pin/pin6.png";
            ImageSmall = "/assets/tiles/50px/pin/pin6.png";
        }
    }
}