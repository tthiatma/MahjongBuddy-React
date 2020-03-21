namespace MahjongBuddy.Core
{
    public class Money2 : Tile
    {
        public Money2()
        {
            Type = TileType.Money;
            Value = TileValue.Two;
            Name = "MoneyTwo";
            Image = "/assets/tiles/64px/man/man2.png";
            ImageSmall = "/assets/tiles/50px/man/man2.png";
        }
    }
}