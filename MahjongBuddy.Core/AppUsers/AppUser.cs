using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace MahjongBuddy.Core.AppUsers
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; }

        public virtual ICollection<UserGame> UserGames { get; set; }
    }
}
