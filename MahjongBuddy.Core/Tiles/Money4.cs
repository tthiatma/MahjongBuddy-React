namespace MahjongBuddy.Core
{
    public class Money4 : Tile
    {
        public Money4()
        {
            Type = TileType.Money;
            Value = TileValue.Four;
            Name = "MoneyFour";
            Image = "/assets/tiles/64px/man/man4.png";
            ImageSmall = "/assets/tiles/50px/man/man4.png";
        }
    }
}