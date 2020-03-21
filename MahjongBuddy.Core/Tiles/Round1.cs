namespace MahjongBuddy.Core
{
    public class Round1 : Tile
    {
        public Round1()
        {
            Id = 11;
            TileType = TileType.Round;
            TileValue = TileValue.One;
            Title = "RoundOne";
            Image = "/assets/tiles/64px/pin/pin1.png";
            ImageSmall = "/assets/tiles/50px/pin/pin1.png";
        }
    }
}