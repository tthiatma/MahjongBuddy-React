using System.Collections.Generic;

namespace MahjongBuddy.Core
{
    public class GamePlayer
    {
        public int Id { get; set; }
        public string PlayerId { get; set; }
        public virtual Player Player { get; set; }
        public int GameId { get; set; }
        public virtual Game Game { get; set; }
        public bool IsHost { get; set; }
        public int Points { get; set; }
        public WindDirection? InitialSeatWind { get; set; }
        public virtual ICollection<Connection> Connections { get; set; }
        public virtual ICollection<RoundPlayer> RoundPlayers { get; set; }

        public GamePlayer()
        {
            Connections = new List<Connection>();
        }
    }
}
