namespace MahjongBuddy.Core
{
    public class Stick9 : Tile
    {
        public Stick9()
        {
            TileType = TileType.Stick;
            TileValue = TileValue.Nine;
            Title = "StickNine";
            Image = "/assets/tiles/64px/bamboo/bamboo9.png";
            ImageSmall = "/assets/tiles/50px/bamboo/bamboo9.png";
        }
    }
}