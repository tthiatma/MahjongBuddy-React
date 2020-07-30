namespace MahjongBuddy.Core
{
    public class RoundPlayerAction
    {
        public int Id { get; set; }

        public ActionType PlayerAction { get; set; }

        public int RoundPlayerId { get; set; }

        public virtual RoundPlayer RoundPlayer { get; set; }
    }
}
