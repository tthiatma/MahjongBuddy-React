using System;

namespace MahjongBuddy.Core
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public virtual Player Player { get; set; }
        public string Token { get; set; }
        public DateTime Expires { get; set; } = DateTime.UtcNow.AddDays(7);
        public bool IsExpired => DateTime.UtcNow >= Expires;
        public DateTime? Revoked { get; set; }
        public bool IsActive => Revoked == null & !IsExpired;
    }
}
