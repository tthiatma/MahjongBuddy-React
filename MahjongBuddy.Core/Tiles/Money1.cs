namespace MahjongBuddy.Core
{
    public class Money1 : Tile
    {
        public Money1()
        {
            Type = TileType.Money;
            Value = TileValue.One;
            Name = "MoneyOne";
            Image = "/assets/tiles/64px/man/man1.png";
            ImageSmall = "/assets/tiles/50px/man/man1.png";
        }
    }
}