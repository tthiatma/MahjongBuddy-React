namespace MahjongBuddy.Core
{
    public class Round8 : Tile
    {
        public Round8()
        {
            Type = TileType.Round;
            Value = TileValue.Eight;
            Name = "RoundEight";
            Image = "/assets/tiles/64px/pin/pin8.png";
            ImageSmall = "/assets/tiles/50px/pin/pin8.png";
        }
    }
}