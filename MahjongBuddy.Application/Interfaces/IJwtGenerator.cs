using MahjongBuddy.Core;

namespace MahjongBuddy.Application.Interfaces
{
    public interface IJwtGenerator
    {
        public string CreateToken(AppUser user);
    }
}
