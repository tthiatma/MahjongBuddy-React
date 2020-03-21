namespace MahjongBuddy.Core
{
    public class Round3 : Tile
    {
        public Round3()
        {
            Id = 13;
            TileType = TileType.Round;
            TileValue = TileValue.Three;
            Title = "RoundThree";
            Image = "/assets/tiles/64px/pin/pin3.png";
            ImageSmall = "/assets/tiles/50px/pin/pin3.png";
        }
    }
}