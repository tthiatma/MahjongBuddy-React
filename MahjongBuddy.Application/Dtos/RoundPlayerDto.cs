﻿using MahjongBuddy.Core;
using System.Collections.Generic;

namespace MahjongBuddy.Application.Dtos
{
    public class RoundPlayerDto
    {
        public string DisplayName { get; set; }

        public bool IsDealer { get; set; }

        public bool IsInitialDealer { get; set; }

        public bool IsManualSort { get; set; }

        public bool IsMyTurn { get; set; }

        public ICollection<RoundPlayerActionDto> RoundPlayerActions { get; set; }

        public ICollection<RoundTileDto> PlayerTiles { get; set; }

        public bool MustThrow { get; set; }

        public WindDirection Wind { get; set; }

        public int Points { get; set; }

    }
}
