namespace MahjongBuddy.Core
{
    public class Money9 : Tile
    {
        public Money9()
        {
            Type = TileType.Money;
            Value = TileValue.Nine;
            Name = "MoneyNine";
            Image = "/assets/tiles/64px/man/man9.png";
            ImageSmall = "/assets/tiles/50px/man/man9.png";
        }
    }
}