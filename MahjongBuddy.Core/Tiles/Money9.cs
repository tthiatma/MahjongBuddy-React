namespace MahjongBuddy.Core
{
    public class Money9 : Tile
    {
        public Money9()
        {
            Id = 9;
            TileType = TileType.Money;
            TileValue = TileValue.Nine;
            Title = "MoneyNine";
            Image = "/assets/tiles/64px/man/man9.png";
            ImageSmall = "/assets/tiles/50px/man/man9.png";
        }
    }
}