namespace MahjongBuddy.Core
{
    public class Round9 : Tile
    {
        public Round9()
        {
            Type = TileType.Round;
            Value = TileValue.Nine;
            Name = "RoundNine";
            Image = "/assets/tiles/64px/pin/pin9.png";
            ImageSmall = "/assets/tiles/50px/pin/pin9.png";
        }
    }
}