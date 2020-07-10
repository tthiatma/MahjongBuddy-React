namespace MahjongBuddy.Core
{
    public class Stick4 : Tile
    {
        public Stick4()
        {
            TileType = TileType.Stick;
            TileValue = TileValue.Four;
            Title = "StickFour";
            Image = "/assets/tiles/64px/bamboo/bamboo4.png";
            ImageSmall = "/assets/tiles/50px/bamboo/bamboo4.png";
        }
    }
}