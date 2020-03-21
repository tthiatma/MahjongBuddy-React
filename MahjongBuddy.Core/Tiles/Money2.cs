namespace MahjongBuddy.Core
{
    public class Money2 : Tile
    {
        public Money2()
        {
            Id = 2;
            TileType = TileType.Money;
            TileValue = TileValue.Two;
            Title = "MoneyTwo";
            Image = "/assets/tiles/64px/man/man2.png";
            ImageSmall = "/assets/tiles/50px/man/man2.png";
        }
    }
}