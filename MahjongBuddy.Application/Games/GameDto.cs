using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MahjongBuddy.Application.Games
{
    public class GameDto
    {
        public int Id { get; set; } 
        
        public string Title { get; set; }

        public DateTime Date { get; set; }

        [JsonPropertyName("players")]
        public ICollection<PlayerDto> UserGames { get; set; }
    }
}
