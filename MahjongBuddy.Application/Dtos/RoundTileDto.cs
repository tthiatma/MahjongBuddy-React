using MahjongBuddy.Core;
using System;

namespace MahjongBuddy.Application.Dtos
{
    public class RoundTileDto
    {
        public Guid Id { get; set; }
        //this is the counter for tile on board graveyard for sorting purposes
        public int BoardGraveyardCounter { get; set; }
        //this is the counter for user active tiles for sorting purposes
        public int ActiveTileCounter { get; set; }
        //userId that own the tile
        public string Owner { get; set; }
        public int RoundId { get; set; }
        //when someone won, mark the tile as winner tile
        public bool IsWinner { get; set; }
        public TileSetGroup TileSetGroup { get; set; }
        public TileStatus Status { get; set; }
        public Tile Tile { get; set; }
    }
}
