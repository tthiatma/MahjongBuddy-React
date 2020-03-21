namespace MahjongBuddy.Core
{
    public class WindSouth : Tile
    {
        public WindSouth()
        {
            Type = TileType.Wind;
            Value = TileValue.WindSouth;
            Name = "WindSouth";
            Image = "/assets/tiles/64px/wind/wind-south.png";
            ImageSmall = "/assets/tiles/50px/wind/wind-south.png";
        }
    }
}