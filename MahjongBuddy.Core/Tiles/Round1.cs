namespace MahjongBuddy.Core
{
    public class Round1 : Tile
    {
        public Round1()
        {
            Type = TileType.Round;
            Value = TileValue.One;
            Name = "RoundOne";
            Image = "/assets/tiles/64px/pin/pin1.png";
            ImageSmall = "/assets/tiles/50px/pin/pin1.png";
        }
    }
}