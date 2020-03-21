namespace MahjongBuddy.Core
{
    public class Money8 : Tile
    {
        public Money8()
        {
            Id = 8;
            TileType = TileType.Money;
            TileValue = TileValue.Eight;
            Title = "MoneyEight";
            Image = "/assets/tiles/64px/man/man8.png";
            ImageSmall = "/assets/tiles/50px/man/man8.png";
        }
    }
}