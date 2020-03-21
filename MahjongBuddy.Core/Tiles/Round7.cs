namespace MahjongBuddy.Core
{
    public class Round7 : Tile
    {
        public Round7()
        {
            Type = TileType.Round;
            Value = TileValue.Seven;
            Name = "RoundSeven";
            Image = "/assets/tiles/64px/pin/pin7.png";
            ImageSmall = "/assets/tiles/50px/pin/pin7.png";
        }
    }
}