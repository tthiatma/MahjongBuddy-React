namespace MahjongBuddy.Core
{
    public class Stick8 : Tile
    {
        public Stick8()
        {
            Id = 28;
            TileType = TileType.Stick;
            TileValue = TileValue.Eight;
            Title = "StickEight";
            Image = "/assets/tiles/64px/bamboo/bamboo8.png";
            ImageSmall = "/assets/tiles/50px/bamboo/bamboo8.png";
        }
    }
}