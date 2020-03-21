namespace MahjongBuddy.Core
{
    public class Round3 : Tile
    {
        public Round3()
        {
            Type = TileType.Round;
            Value = TileValue.Three;
            Name = "RoundThree";
            Image = "/assets/tiles/64px/pin/pin3.png";
            ImageSmall = "/assets/tiles/50px/pin/pin3.png";
        }
    }
}