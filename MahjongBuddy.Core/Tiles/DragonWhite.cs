namespace MahjongBuddy.Core
{
    public class DragonWhite: Tile
    {
        public DragonWhite()
        {
            Type = TileType.Dragon;
            Value = TileValue.DragonWhite;
            Name = "DragonWhite";
            Image = "/assets/tiles/64px/dragon/dragon-white.png";
            ImageSmall = "/assets/tiles/50px/dragon/dragon-white.png";
        }
    }
}