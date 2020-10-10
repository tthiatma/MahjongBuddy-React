namespace MahjongBuddy.Core
{
    /// <summary>
    /// SignalR connection
    /// </summary>
    public class Connection
    {
        //ConnectionId
        public string Id { get; set; }
        public string UserAgent { get; set; }
        public bool Connected { get; set; }
    }
}
