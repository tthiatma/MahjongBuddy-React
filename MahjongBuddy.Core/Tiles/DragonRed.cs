﻿namespace MahjongBuddy.Core
{
    public class DragonRed : Tile
    {
        public DragonRed()
        {
            TileType = TileType.Dragon;
            TileValue = TileValue.DragonRed;
            Title = "DragonRed";
            Image = "/assets/tiles/64px/dragon/dragon-chun.png";
            ImageSmall = "/assets/tiles/50px/dragon/dragon-chun.png";
        }
    }
}