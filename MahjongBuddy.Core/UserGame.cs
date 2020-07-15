namespace MahjongBuddy.Core
{
    public class UserGame
    {
        public string AppUserId { get; set; }
        public virtual AppUser AppUser { get; set; }
        public int GameId { get; set; }
        public virtual Game Game { get; set; }
        public bool IsHost { get; set; }
        public int Points { get; set; }
        public WindDirection InitialSeatWind { get; set; }
    }
}
