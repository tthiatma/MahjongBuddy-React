namespace MahjongBuddy.Core
{
    public class Money5 : Tile
    {
        public Money5()
        {
            TileType = TileType.Money;
            TileValue = TileValue.Five;
            Title = "MoneyFive";
            Image = "/assets/tiles/64px/man/man5.png";
            ImageSmall = "/assets/tiles/50px/man/man5.png";
        }
    }
}