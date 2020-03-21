namespace MahjongBuddy.Core
{
    public class WindNorth: Tile
    {
        public WindNorth()
        {
            Id = 44;
            TileType = TileType.Wind;
            TileValue = TileValue.WindNorth;
            Title = "WindNorth";
            Image = "/assets/tiles/64px/wind/wind-north.png";
            ImageSmall = "/assets/tiles/50px/wind/wind-north.png";
        }
    }
}