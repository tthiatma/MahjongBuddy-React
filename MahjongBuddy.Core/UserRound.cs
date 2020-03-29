using System.Collections.Generic;

namespace MahjongBuddy.Core
{
    public class UserRound
    {
        public string AppUserId { get; set; }
        public virtual AppUser AppUser { get; set; }
        public int RoundId { get; set; }
        public virtual Round Round { get; set; }
        public bool CanPickTile { get; set; }
        public bool CanThrowTile { get; set; }
        public bool CanDoNoFlower { get; set; }
    }
}
