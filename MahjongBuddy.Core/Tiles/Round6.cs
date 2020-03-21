namespace MahjongBuddy.Core
{
    public class Round6 : Tile
    {
        public Round6()
        {
            Type = TileType.Round;
            Value = TileValue.Six;
            Name = "RoundSix";
            Image = "/assets/tiles/64px/pin/pin6.png";
            ImageSmall = "/assets/tiles/50px/pin/pin6.png";
        }
    }
}