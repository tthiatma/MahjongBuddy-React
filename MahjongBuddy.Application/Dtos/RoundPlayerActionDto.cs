﻿using MahjongBuddy.Core;

namespace MahjongBuddy.Application.Dtos
{
    public class RoundPlayerActionDto
    {
        public int Id { get; set; }

        public ActionType PlayerAction { get; set; }
    }
}
