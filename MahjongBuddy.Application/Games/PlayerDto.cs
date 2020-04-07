using MahjongBuddy.Core;

namespace MahjongBuddy.Application.Games
{
    public class PlayerDto
    {
        public string UserName { get; set; }

        public string DisplayName { get; set; }

        public WindDirection InitialSeatWind { get; set; }

        public string Image { get; set; }

        public bool IsHost { get; set; }
    }
}
