using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MahjongBuddy.Core.AppUsers
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; }
    }
}
