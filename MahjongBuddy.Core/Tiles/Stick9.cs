namespace MahjongBuddy.Core
{
    public class Stick9 : Tile
    {
        public Stick9()
        {
            Type = TileType.Stick;
            Value = TileValue.Nine;
            Name = "StickNine";
            Image = "/assets/tiles/64px/bamboo/bamboo9.png";
            ImageSmall = "/assets/tiles/50px/bamboo/bamboo9.png";
        }
    }
}