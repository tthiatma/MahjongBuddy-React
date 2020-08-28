using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace MahjongBuddy.Core
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; }

        public string Bio { get; set; }

        public virtual ICollection<UserGame> UserGames { get; set; }

        public virtual ICollection<RoundPlayer> UserRounds { get; set; }

        public virtual ICollection<Photo> Photos { get; set; }

        public string RefreshToken { get; set; }

        public DateTime RefreshTokenExpiry { get; set; }
    }
}
