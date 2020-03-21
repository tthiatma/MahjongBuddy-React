namespace MahjongBuddy.Core
{
    public class Money8 : Tile
    {
        public Money8()
        {
            Type = TileType.Money;
            Value = TileValue.Eight;
            Name = "MoneyEight";
            Image = "/assets/tiles/64px/man/man8.png";
            ImageSmall = "/assets/tiles/50px/man/man8.png";
        }
    }
}