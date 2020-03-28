using System;

namespace MahjongBuddy.Core
{
    public class ChatMsg
    {
        public Guid Id { get; set; }

        public string Body { get; set; }

        public AppUser Author { get; set; }

        public Game Game { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
