using MahjongBuddy.Core.Enums;
using System.Collections.Generic;

namespace MahjongBuddy.Core
{
    public class RoundResult
    {
        public int Id { get; set; }
        //IsWinner if false, then the user here feed
        public PlayerResult PlayerResult { get; set; }
        public virtual Round Round { get; set; }
        public int RoundId { get; set; }
        public virtual AppUser AppUser { get; set; }
        public string  AppUserId { get; set; }
        public virtual ICollection<RoundResultHand> RoundResultHands { get; set; }
        public virtual ICollection<RoundResultExtraPoint> RoundResultExtraPoints { get; set; }
        public int PointsResult { get; set; }

        public RoundResult()
        {
            RoundResultHands = new List<RoundResultHand>();
            RoundResultExtraPoints = new List<RoundResultExtraPoint>();
        }
    }
}
