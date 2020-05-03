using MahjongBuddy.Core;
using System.Collections.Generic;

namespace MahjongBuddy.Application.Helpers
{
    public static class TileHelper
    {
        public static List<Tile> BuildThirteenWonder()
        {
            List<Tile> tiles = new List<Tile>();
            tiles.Add(new Money1());
            tiles.Add(new Money9());
            tiles.Add(new Circle1());
            tiles.Add(new Circle9());
            tiles.Add(new Stick1());
            tiles.Add(new Stick9());
            tiles.Add(new WindEast());
            tiles.Add(new WindSouth());
            tiles.Add(new WindWest());
            tiles.Add(new WindNorth());
            tiles.Add(new DragonGreen());
            tiles.Add(new DragonRed());
            tiles.Add(new DragonWhite());
            return tiles;
        }
    }
}
