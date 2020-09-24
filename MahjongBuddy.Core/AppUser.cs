using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MahjongBuddy.Core
{
    public class AppUser : IdentityUser
    {
        public AppUser()
        {
            Photos = new Collection<Photo>();
        }

        public string DisplayName { get; set; }

        public string Bio { get; set; }

        public virtual ICollection<GamePlayer> UserGames { get; set; }

        public virtual ICollection<RoundPlayer> UserRounds { get; set; }

        public virtual ICollection<Photo> Photos { get; set; }

        public string RefreshToken { get; set; }

        public DateTime RefreshTokenExpiry { get; set; }
    }
}
