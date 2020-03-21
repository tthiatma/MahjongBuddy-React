namespace MahjongBuddy.Core
{
    public class Round8 : Tile
    {
        public Round8()
        {
            Id = 18;
            TileType = TileType.Round;
            TileValue = TileValue.Eight;
            Title = "RoundEight";
            Image = "/assets/tiles/64px/pin/pin8.png";
            ImageSmall = "/assets/tiles/50px/pin/pin8.png";
        }
    }
}