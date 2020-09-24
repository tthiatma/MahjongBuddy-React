using MahjongBuddy.Core.Enums;
using System;
using System.Collections.Generic;

namespace MahjongBuddy.Core
{
    public class Game
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int MinPoint { get; set; }
        public int MaxPoint { get; set; }
        public DateTime Date { get; set; }
        public GameStatus Status { get; set; }
        public string HostId { get; set; }
        public virtual AppUser Host { get; set; }
        public virtual ICollection<Round> Rounds { get; set; }
        public virtual ICollection<GamePlayer> GamePlayers { get; set; }
        public virtual ICollection<ChatMsg> ChatMsgs { get; set; }
    }
}
