using MahjongBuddy.Core;
using MahjongBuddy.Core.Enums;
using System.Collections.Generic;

namespace MahjongBuddy.Application.Rounds.Scorings
{
    public class HandWorth
    {
        public ICollection<HandType> HandTypes { get; set; }
        public ICollection<ExtraPoint> ExtraPoints { get; set; }
        public int Points { get; set; }
    }
}
