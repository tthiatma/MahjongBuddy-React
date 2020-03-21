namespace MahjongBuddy.Core
{
    public class Round4 : Tile
    {
        public Round4()
        {
            Id = 14;
            TileType = TileType.Round;
            TileValue = TileValue.Four;
            Title = "RoundFour";
            Image = "/assets/tiles/64px/pin/pin4.png";
            ImageSmall = "/assets/tiles/50px/pin/pin4.png";
        }
    }
}