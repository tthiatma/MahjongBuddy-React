namespace MahjongBuddy.Core
{
    public class Round9 : Tile
    {
        public Round9()
        {
            Id = 19;
            TileType = TileType.Round;
            TileValue = TileValue.Nine;
            Title = "RoundNine";
            Image = "/assets/tiles/64px/pin/pin9.png";
            ImageSmall = "/assets/tiles/50px/pin/pin9.png";
        }
    }
}