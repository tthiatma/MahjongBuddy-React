﻿using MahjongBuddy.Core;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MahjongBuddy.Application.Dtos
{
    public class RoundDto
    {
        public int Id { get; set; }
        public int RoundCounter { get; set; }
        public int TileCounter { get; set; }
        public WindDirection Wind { get; set; }
        public DateTime DateCreated { get; set; }
        //IsHalted is a short period where all player have brief moment of time to chow or pong or kong
        public bool IsHalted { get; set; }
        public bool IsOver { get; set; }
        public bool IsPaused { get; set; }
        public bool IsTied { get; set; }
        public bool IsWinnerSelfPicked { get; set; }
        public int GameId { get; set; }
        public virtual ICollection<RoundTileDto> RoundTiles { get; set; }

        //public virtual ICollection<RoundResult> RoundResults { get; set; }

        [JsonPropertyName("roundPlayers")]
        public virtual ICollection<RoundPlayerDto> UserRounds { get; set; }

        //public virtual ICollection<UserGame> UserGames { get; set; }

    }
}