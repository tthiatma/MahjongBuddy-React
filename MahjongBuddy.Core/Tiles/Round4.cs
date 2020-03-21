namespace MahjongBuddy.Core
{
    public class Round4 : Tile
    {
        public Round4()
        {
            Type = TileType.Round;
            Value = TileValue.Four;
            Name = "RoundFour";
            Image = "/assets/tiles/64px/pin/pin4.png";
            ImageSmall = "/assets/tiles/50px/pin/pin4.png";
        }
    }
}