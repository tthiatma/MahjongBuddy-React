﻿using MahjongBuddy.Core;
using System.Collections.Generic;

namespace MahjongBuddy.Application.Dtos
{
    public class RoundPlayerDto : GamePlayerDto
    {
        public bool IsDealer { get; set; }

        public bool IsInitialDealer { get; set; }

        public bool IsMyTurn { get; set; }

        public bool MustThrow { get; set; }

        public WindDirection Wind { get; set; }

        public bool IsManualSort { get; set; }

        public ICollection<RoundPlayerActionDto> RoundPlayerActiveActions { get; set; }

        public ICollection<RoundTileDto> PlayerTiles { get; set; }
    }
}
