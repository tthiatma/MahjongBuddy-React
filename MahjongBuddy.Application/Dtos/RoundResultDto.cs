﻿using System.Collections.Generic;

namespace MahjongBuddy.Application.Dtos
{
    public class RoundResultDto
    {
        public int Id { get; set; }
        public bool IsWinner { get; set; }
        public string UserName { get; set; }
        public ICollection<RoundResultHandDto> RoundResultHands { get; set; }
        public ICollection<RoundResultExtraPointDto> RoundResultExtraPoints { get; set; }
        public int PointsResult { get; set; }
    }
}