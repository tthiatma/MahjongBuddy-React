namespace MahjongBuddy.Core
{
    public class DragonWhite: Tile
    {
        public DragonWhite()
        {
            Id = 33;
            TileType = TileType.Dragon;
            TileValue = TileValue.DragonWhite;
            Title = "DragonWhite";
            Image = "/assets/tiles/64px/dragon/dragon-white.png";
            ImageSmall = "/assets/tiles/50px/dragon/dragon-white.png";
        }
    }
}