﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MahjongBuddy.Core
{
    public class Round
    {
        public int Id { get; set; }
        public int RoundCounter { get; set; }
        /// <summary>
        /// TileCounter is to order tile in board graveyard
        /// </summary>
        public int TileCounter { get; set; }
        public WindDirection Wind { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsOver { get; set; }
        public bool IsEnding { get; set; }
        public bool IsPaused { get; set; }
        public bool IsTied { get; set; }
        public int GameId { get; set; }
        public virtual Game Game { get; set; }
        public virtual ICollection<RoundResult> RoundResults { get; set; }
        public virtual ICollection<RoundPlayer> RoundPlayers { get; set; }
        [ConcurrencyCheck]
        public virtual ICollection<RoundTile> RoundTiles { get; set; }
    }
}
