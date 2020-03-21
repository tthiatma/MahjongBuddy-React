namespace MahjongBuddy.Core
{
    public class Money5 : Tile
    {
        public Money5()
        {
            Type = TileType.Money;
            Value = TileValue.Five;
            Name = "MoneyFive";
            Image = "/assets/tiles/64px/man/man5.png";
            ImageSmall = "/assets/tiles/50px/man/man5.png";
        }
    }
}