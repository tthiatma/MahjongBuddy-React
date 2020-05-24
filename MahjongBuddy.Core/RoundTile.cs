using System;
using System.ComponentModel.DataAnnotations;

namespace MahjongBuddy.Core
{
    public class RoundTile
    {
        public Guid Id { get; set; }
        //this is the counter for tile on board graveyard for sorting purposes
        public int BoardGraveyardCounter { get; set; }
        //this is the counter for user active tiles for sorting purposes
        public int ActiveTileCounter { get; set; }
        public string Owner { get; set; }
        //track who thrown the tile
        public string ThrownBy { get; set; }
        public int RoundId { get; set; }
        public TileSetGroup TileSetGroup { get; set; }
        public int TileSetGroupIndex { get; set; }
        public TileStatus Status { get; set; }
        public virtual Round Round { get; set; }
        public virtual Tile Tile { get; set; }
        [Timestamp]
        public byte[] Timestamp { get; set; }
    }
}
