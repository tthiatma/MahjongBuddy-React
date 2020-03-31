﻿using System.Collections.Generic;

namespace MahjongBuddy.Core
{
    public class Round
    {
        public int Id { get; set; }
        public int Index { get; set; }
        public WindDirection Wind { get; set; }
        //IsHalted is a short period where all player have brief moment of time to chow or pong or kong
        public bool IsHalted { get; set; }
        public bool IsStarted { get; set; }
        public bool IsOver { get; set; }
        public bool IsPaused { get; set; }
        public bool IsTied { get; set; }
        public bool IsWinnerSelfPicked { get; set; }
        public int GameId { get; set; }
        public virtual Game Game { get; set; }
        public string DiceThrowerAppUserId { get; set; }
        //the user that is a dice thrower will have 14 tiles initially in hk mahjong
        public virtual AppUser DiceThrowerAppUser { get; set; }
        public string TurnAppUserId { get; set; }
        public virtual AppUser TurnAppUser { get; set; }
        public virtual ICollection<RoundResult> RoundResults { get; set; }
        public virtual ICollection<RoundTile> PlayTiles { get; set; }
        public virtual ICollection<UserRound> UserRounds { get; set; }
        public virtual ICollection<UserGame> UserGames { get; set; }
    }
}
