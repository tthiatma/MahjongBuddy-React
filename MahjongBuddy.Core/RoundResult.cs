using MahjongBuddy.Core.Enums;
using System.Collections.Generic;

namespace MahjongBuddy.Core
{
    public class RoundResult
    {
        public int Id { get; set; }
        public PlayResult PlayResult { get; set; }
        public virtual Round Round { get; set; }
        public int RoundId { get; set; }
        public virtual Player Player { get; set; }
        public string  PlayerId { get; set; }
        public virtual ICollection<RoundResultHand> RoundResultHands { get; set; }
        public virtual ICollection<RoundResultExtraPoint> RoundResultExtraPoints { get; set; }
        public int Points { get; set; }

        public RoundResult()
        {
            RoundResultHands = new List<RoundResultHand>();
            RoundResultExtraPoints = new List<RoundResultExtraPoint>();
        }
    }
}
