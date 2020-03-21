namespace MahjongBuddy.Core
{
    public class DragonGreen : Tile
    {
        public DragonGreen()
        {
            Type = TileType.Dragon;
            Value = TileValue.DragonGreen;
            Name = "DragonGreen";
            Image = "/assets/tiles/64px/dragon/dragon-green.png";
            ImageSmall = "/assets/tiles/50px/dragon/dragon-green.png";
        }
    }
}