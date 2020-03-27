using System;

namespace MahjongBuddy.Core
{
    public class GameTile
    {
        public Guid Id { get; set; }

        public int TileId { get; set; }

        public virtual Tile Tile { get; set; }

        public virtual Game Game { get; set; }

        public int GameId { get; set; }

        public string Owner { get; set; }
        
        public TileStatus Status { get; set; }

        //this is the counter for tile on board graveyard
        public int OpenTileCounter { get; set; }
 
        //this is the tile counter for user active tiles
        public int ActiveTileIndex { get; set; }
    }
}
