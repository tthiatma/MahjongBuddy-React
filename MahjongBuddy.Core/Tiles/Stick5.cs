namespace MahjongBuddy.Core
{
    public class Stick5 : Tile
    {
        public Stick5()
        {
            Type = TileType.Stick;
            Value = TileValue.Five;
            Name = "StickFive";
            Image = "/assets/tiles/64px/bamboo/bamboo5.png";
            ImageSmall = "/assets/tiles/50px/bamboo/bamboo5.png";
        }
    }
}