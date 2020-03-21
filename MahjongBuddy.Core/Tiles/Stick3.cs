namespace MahjongBuddy.Core
{
    public class Stick3 : Tile
    {
        public Stick3()
        {
            Type = TileType.Stick;
            Value = TileValue.Three;
            Name = "StickThree";
            Image = "/assets/tiles/64px/bamboo/bamboo3.png";
            ImageSmall = "/assets/tiles/50px/bamboo/bamboo3.png";
        }
    }
}