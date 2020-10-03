﻿using MahjongBuddy.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MahjongBuddy.Application.Dtos
{
    public class GameDto
    {
        public int Id { get; set; } 
        
        public string Title { get; set; }

        public int MinPoint { get; set; }

        public int MaxPoint { get; set; }

        public DateTime Date { get; set; }

        public GameStatus Status { get; set; }

        public string HostUserName { get; set; }

        [JsonPropertyName("players")]
        public ICollection<PlayerDto> GamePlayers { get; set; }

        public ICollection<ChatMsgDto> ChatMsgs { get; set; }
    }
}
