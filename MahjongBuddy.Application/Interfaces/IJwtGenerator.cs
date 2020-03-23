using MahjongBuddy.Core.AppUsers;
using System;
using System.Collections.Generic;
using System.Text;

namespace MahjongBuddy.Application.Interfaces
{
    public interface IJwtGenerator
    {
        public string CreateToken(AppUser user);
    }
}
