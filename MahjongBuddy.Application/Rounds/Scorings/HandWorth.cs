using MahjongBuddy.Core;
using MahjongBuddy.Core.Enums;
using System.Collections.Generic;

namespace MahjongBuddy.Application.Rounds.Scorings
{
    public class HandWorth
    {
        public IEnumerable<HandType> HandTypes { get; set; }
        public IEnumerable<ExtraPoint> ExtraPoints { get; set; }
        public int Points { get; set; }
    }
}
