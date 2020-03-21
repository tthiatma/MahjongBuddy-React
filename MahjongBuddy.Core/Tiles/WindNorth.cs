namespace MahjongBuddy.Core
{
    public class WindNorth: Tile
    {
        public WindNorth()
        {
            Type = TileType.Wind;
            Value = TileValue.WindNorth;
            Name = "WindNorth";
            Image = "/assets/tiles/64px/wind/wind-north.png";
            ImageSmall = "/assets/tiles/50px/wind/wind-north.png";
        }
    }
}