﻿namespace MahjongBuddy.Core
{
    public class WindWest : Tile
    {
        public WindWest()
        {
            TileType = TileType.Wind;
            TileValue = TileValue.WindWest;
            Title = "WindWest";
            Image = "/assets/tiles/64px/wind/wind-west.png";
            ImageSmall = "/assets/tiles/50px/wind/wind-west.png";
        }
    }
}