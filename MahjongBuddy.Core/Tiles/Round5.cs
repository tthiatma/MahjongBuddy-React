namespace MahjongBuddy.Core
{
    public class Round5 : Tile
    {
        public Round5()
        {
            Type = TileType.Round;
            Value = TileValue.Five;
            Name = "RoundFive";
            Image = "/assets/tiles/64px/pin/pin5.png";
            ImageSmall = "/assets/tiles/50px/pin/pin5.png";
        }
    }
}