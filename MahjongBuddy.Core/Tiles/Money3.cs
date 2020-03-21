namespace MahjongBuddy.Core
{
    public class Money3 : Tile
    {
        public Money3()
        {
            Type = TileType.Money;
            Value = TileValue.Three;
            Name = "MoneyThree";
            Image = "/assets/tiles/64px/man/man3.png";
            ImageSmall = "/assets/tiles/50px/man/man3.png";
        }
    }
}