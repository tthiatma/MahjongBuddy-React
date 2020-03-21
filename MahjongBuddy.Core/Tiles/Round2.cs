namespace MahjongBuddy.Core
{
    public class Round2 : Tile
    {
        public Round2()
        {
            Type = TileType.Round;
            Value = TileValue.Two;
            Name = "RoundTwo";
            Image = "/assets/tiles/64px/pin/pin2.png";
            ImageSmall = "/assets/tiles/50px/pin/pin2.png";
        }
    }
}