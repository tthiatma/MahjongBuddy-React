namespace MahjongBuddy.Core
{
    public class Stick3 : Tile
    {
        public Stick3()
        {
            Id = 23;
            TileType = TileType.Stick;
            TileValue = TileValue.Three;
            Title = "StickThree";
            Image = "/assets/tiles/64px/bamboo/bamboo3.png";
            ImageSmall = "/assets/tiles/50px/bamboo/bamboo3.png";
        }
    }
}