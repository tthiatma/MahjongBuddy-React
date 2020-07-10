namespace MahjongBuddy.Core
{
    public class WindEast : Tile
    {
        public WindEast()
        {
            TileType = TileType.Wind;
            TileValue = TileValue.WindEast;
            Title = "WindEast";
            Image = "/assets/tiles/64px/wind/wind-east.png";
            ImageSmall = "/assets/tiles/50px/wind/wind-east.png";
        }
    }
}