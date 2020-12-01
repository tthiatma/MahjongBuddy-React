namespace MahjongBuddy.Application.Dtos
{
    public class ConnectionDto
    {
        //ConnectionId
        public string Id { get; set; }
        public int GamePlayerId { get; set; }
        public string UserAgent { get; set; }
        public bool Connected { get; set; }
    }
}
