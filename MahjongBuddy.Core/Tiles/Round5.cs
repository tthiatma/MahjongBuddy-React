namespace MahjongBuddy.Core
{
    public class Round5 : Tile
    {
        public Round5()
        {
            Id = 15;
            TileType = TileType.Round;
            TileValue = TileValue.Five;
            Title = "RoundFive";
            Image = "/assets/tiles/64px/pin/pin5.png";
            ImageSmall = "/assets/tiles/50px/pin/pin5.png";
        }
    }
}