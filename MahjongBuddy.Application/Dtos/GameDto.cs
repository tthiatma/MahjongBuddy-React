using MahjongBuddy.Application.ChatMsgs;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MahjongBuddy.Application.Dtos
{
    public class GameDto
    {
        public int Id { get; set; } 
        
        public string Title { get; set; }

        public DateTime Date { get; set; }

        public string HostUserName { get; set; }

        [JsonPropertyName("players")]
        public ICollection<PlayerDto> UserGames { get; set; }

        public ICollection<ChatMsgDto> ChatMsgs { get; set; }
    }
}
