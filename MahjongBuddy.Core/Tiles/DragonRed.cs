namespace MahjongBuddy.Core
{
    public class DragonRed : Tile
    {
        public DragonRed()
        {
            Type = TileType.Dragon;
            Value = TileValue.DragonRed;
            Name = "DragonRed";
            Image = "/assets/tiles/64px/dragon/dragon-chun.png";
            ImageSmall = "/assets/tiles/50px/dragon/dragon-chun.png";
        }
    }
}