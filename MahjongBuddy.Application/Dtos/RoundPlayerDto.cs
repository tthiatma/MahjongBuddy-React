using MahjongBuddy.Core;
using System.Collections.Generic;

namespace MahjongBuddy.Application.Dtos
{
    public class RoundPlayerDto
    {
        public string UserName { get; set; }

        public string DisplayName { get; set; }

        public bool IsDealer { get; set; }

        public bool IsMyTurn { get; set; }

        public WindDirection Wind { get; set; }

        public int Points { get; set; }

    }
}
