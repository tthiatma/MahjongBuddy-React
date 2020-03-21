namespace MahjongBuddy.Core
{
    public class WindWest : Tile
    {
        public WindWest()
        {
            Type = TileType.Wind;
            Value = TileValue.WindWest;
            Name = "WindWest";
            Image = "/assets/tiles/64px/wind/wind-west.png";
            ImageSmall = "/assets/tiles/50px/wind/wind-west.png";
        }
    }
}