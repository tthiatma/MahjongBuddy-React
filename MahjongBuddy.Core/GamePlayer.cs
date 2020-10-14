using System.Collections.Generic;

namespace MahjongBuddy.Core
{
    public class GamePlayer
    {
        public string AppUserId { get; set; }
        public virtual AppUser AppUser { get; set; }
        public int GameId { get; set; }
        public virtual Game Game { get; set; }
        public bool IsHost { get; set; }
        public int Points { get; set; }
        public WindDirection? InitialSeatWind { get; set; }
        public virtual ICollection<Connection> Connections { get; set; }

        public GamePlayer()
        {
            Connections = new List<Connection>();
        }
    }
}
