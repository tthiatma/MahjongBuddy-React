namespace MahjongBuddy.Core
{
    public class Money1 : Tile
    {
        public Money1()
        {
            Id = 1;
            TileType = TileType.Money;
            TileValue = TileValue.One;
            Title = "MoneyOne";
            Image = "/assets/tiles/64px/man/man1.png";
            ImageSmall = "/assets/tiles/50px/man/man1.png";
        }
    }
}