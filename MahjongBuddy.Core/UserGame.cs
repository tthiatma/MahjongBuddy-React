using System;
using System.Collections.Generic;
using System.Text;

namespace MahjongBuddy.Core
{
    public class UserGame
    {
        public string AppUserId { get; set; }
        public virtual AppUser AppUser { get; set; }
        public int GameId { get; set; }
        public virtual Game Game { get; set; }
        public bool IsHost { get; set; }
        public bool IsPlaying { get; set; }
        public bool CanPickTile { get; set; }
        public bool CanThrowTile { get; set; }
        public bool CanDoNoFlower { get; set; }
        public int CurrentPoint { get; set; }
    }
}
