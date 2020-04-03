namespace MahjongBuddy.Core
{
    public class RoundResult
    {
        public int Id { get; set; }
        //IsWinner if false, then the user here feed
        public bool IsWinner { get; set; }
        public virtual Round Round { get; set; }
        public virtual AppUser AppUser { get; set; }
        public string  AppUserId { get; set; }
    }
}
