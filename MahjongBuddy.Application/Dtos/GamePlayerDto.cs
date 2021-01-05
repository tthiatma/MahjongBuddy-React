using MahjongBuddy.Core;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MahjongBuddy.Application.Dtos
{
    public class GamePlayerDto
    {
        public string UserName { get; set; }

        public string DisplayName { get; set; }

        public WindDirection? InitialSeatWind { get; set; }

        public int Points { get; set; }

        public string Image { get; set; }

        public bool IsHost { get; set; }

        [JsonIgnore]
        public IEnumerable<ConnectionDto> Connections { get; set; }
    }
}