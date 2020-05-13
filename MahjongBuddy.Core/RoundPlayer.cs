namespace MahjongBuddy.Core
{
    public class RoundPlayer
    {
        public string AppUserId { get; set; }
        public virtual AppUser AppUser { get; set; }
        public int RoundId { get; set; }
        public virtual Round Round { get; set; }
        public bool IsDealer { get; set; }
        public bool IsMyTurn { get; set; }
        public WindDirection Wind { get; set; }
        public int Points { get; set; }
    }
}
