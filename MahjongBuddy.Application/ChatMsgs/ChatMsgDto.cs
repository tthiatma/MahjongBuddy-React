using System;

namespace MahjongBuddy.Application.ChatMsgs
{
    public class ChatMsgDto
    {
        public Guid Id { get; set; }

        public string Body { get; set; }

        public DateTime CreatedAt { get; set; }

        public string UserName { get; set; }

        public string DisplayName { get; set; }

        public string Image { get; set; }

    }
}
