using System;

namespace MahjongBuddy.Core
{
    public class PlayTile
    {
        public Guid Id { get; set; }
        //this is the counter for tile on board graveyard for sorting purposes
        public int BoardGraveyardCounter { get; set; }
        //this is the counter for user active tiles for sorting purposes
        public int ActiveTileIndex { get; set; }
        public string Owner { get; set; }
        public int GameId { get; set; }
        //when someone won, mark the tile as winner tile
        public bool IsWinner { get; set; }
        public TileSetGroup TileSetGroup { get; set; }
        public TileStatus Status { get; set; }
        public virtual Game Game { get; set; }
        public virtual Tile Tile { get; set; }
    }
}
