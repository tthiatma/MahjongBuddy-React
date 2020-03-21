using System;

namespace MahjongBuddy.Core
{
    public class GameTile
    {
        public Guid Id { get; set; }

        public Tile Tile { get; set; }

        public Game Game { get; set; }

        public string Owner { get; set; }
        
        public TileStatus Status { get; set; }

        //this is the counter for tile on board graveyard
        public int OpenTileCounter { get; set; }
 
        //this is the tile counter for user active tiles
        public int ActiveTileIndex { get; set; }
    }
}
