using MahjongBuddy.Core;
using System;
using System.Collections.Generic;

namespace MahjongBuddy.Application.Dtos
{
    public class RoundDto
    {
        public int Id { get; set; }
        public int RoundCounter { get; set; }
        public int TileCounter { get; set; }
        public WindDirection Wind { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsOver { get; set; }
        public bool IsEnding { get; set; }
        public bool IsPaused { get; set; }
        public bool IsTied { get; set; }
        public int GameId { get; set; }
        public int RemainingTiles { get; set; }
        public ICollection<RoundResultDto> RoundResults { get; set; }
        public ICollection<RoundTileDto> BoardTiles { get; set; }
        //below props need to be calculated
        public RoundPlayerDto MainPlayer { get; set; }
        public ICollection<RoundOtherPlayerDto> OtherPlayers{ get; set; }
    }
}
