using MahjongBuddy.Core.Enums;
using System.Collections.Generic;

namespace MahjongBuddy.Application.Dtos
{
    public class RoundResultDto
    {
        public int Id { get; set; }
        public PlayResult PlayResult { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public ICollection<RoundResultHandDto> RoundResultHands { get; set; }
        public ICollection<RoundResultExtraPointDto> RoundResultExtraPoints { get; set; }
        public ICollection<RoundTileDto> PlayerTiles { get; set; }
        public int Points { get; set; }
    }
}
