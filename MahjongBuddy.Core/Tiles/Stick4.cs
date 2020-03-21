namespace MahjongBuddy.Core
{
    public class Stick4 : Tile
    {
        public Stick4()
        {
            Type = TileType.Stick;
            Value = TileValue.Four;
            Name = "StickFour";
            Image = "/assets/tiles/64px/bamboo/bamboo4.png";
            ImageSmall = "/assets/tiles/50px/bamboo/bamboo4.png";
        }
    }
}