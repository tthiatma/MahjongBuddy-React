namespace MahjongBuddy.Core
{
    /// <summary>
    /// SignalR connection
    /// </summary>
    public class Connection
    {
        //ConnectionId
        public string Id { get; set; }
        public int GamePlayerId { get; set; }
        public virtual GamePlayer GamePlayer { get; set; }
        public string UserAgent { get; set; }
        public bool Connected { get; set; }
    }
}
