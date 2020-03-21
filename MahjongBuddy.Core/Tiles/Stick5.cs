namespace MahjongBuddy.Core
{
    public class Stick5 : Tile
    {
        public Stick5()
        {
            Id = 25;
            TileType = TileType.Stick;
            TileValue = TileValue.Five;
            Title = "StickFive";
            Image = "/assets/tiles/64px/bamboo/bamboo5.png";
            ImageSmall = "/assets/tiles/50px/bamboo/bamboo5.png";
        }
    }
}