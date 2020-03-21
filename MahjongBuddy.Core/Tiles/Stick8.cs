namespace MahjongBuddy.Core
{
    public class Stick8 : Tile
    {
        public Stick8()
        {
            Type = TileType.Stick;
            Value = TileValue.Eight;
            Name = "StickEight";
            Image = "/assets/tiles/64px/bamboo/bamboo8.png";
            ImageSmall = "/assets/tiles/50px/bamboo/bamboo8.png";
        }
    }
}