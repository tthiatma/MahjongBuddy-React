namespace MahjongBuddy.Core
{
    public class Round7 : Tile
    {
        public Round7()
        {
            Id = 17;
            TileType = TileType.Round;
            TileValue = TileValue.Seven;
            Title = "RoundSeven";
            Image = "/assets/tiles/64px/pin/pin7.png";
            ImageSmall = "/assets/tiles/50px/pin/pin7.png";
        }
    }
}