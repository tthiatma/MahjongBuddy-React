namespace MahjongBuddy.Application.Dtos
{
    public class OnDisconnectedDto
    {
        public GamePlayerDto GamePlayer { get; set; }

        public GameDto Game { get; set; }
    }
}
