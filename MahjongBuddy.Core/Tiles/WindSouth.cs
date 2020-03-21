namespace MahjongBuddy.Core
{
    public class WindSouth : Tile
    {
        public WindSouth()
        {
            Id = 42;
            TileType = TileType.Wind;
            TileValue = TileValue.WindSouth;
            Title = "WindSouth";
            Image = "/assets/tiles/64px/wind/wind-south.png";
            ImageSmall = "/assets/tiles/50px/wind/wind-south.png";
        }
    }
}