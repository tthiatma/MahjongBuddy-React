using MahjongBuddy.Core.Enums;

namespace MahjongBuddy.Application.Dtos
{
    public class RoundResultExtraPointDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ExtraPoint ExtraPoint { get; set; }
        public int Point { get; set; }
    }
}
