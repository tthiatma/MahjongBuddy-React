namespace MahjongBuddy.Core
{
    public class WindEast : Tile
    {
        public WindEast()
        {
            Type = TileType.Wind;
            Value = TileValue.WindEast;
            Name = "WindEast";
            Image = "/assets/tiles/64px/wind/wind-east.png";
            ImageSmall = "/assets/tiles/50px/wind/wind-east.png";
        }
    }
}