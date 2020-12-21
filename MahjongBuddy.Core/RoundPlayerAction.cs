using MahjongBuddy.Core.Enums;

namespace MahjongBuddy.Core
{
    public class RoundPlayerAction
    {
        public int Id { get; set; }

        public ActionType ActionType { get; set; }

        public ActionStatus ActionStatus { get; set; }

        public int RoundPlayerId { get; set; }

        public virtual RoundPlayer RoundPlayer { get; set; }
    }
}
