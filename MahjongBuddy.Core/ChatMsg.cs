using System;

namespace MahjongBuddy.Core
{
    public class ChatMsg
    {
        public Guid Id { get; set; }

        public string Body { get; set; }

        public virtual AppUser Author { get; set; }

        public virtual Game Game { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
