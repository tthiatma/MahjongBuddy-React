using MahjongBuddy.Core;

namespace MahjongBuddy.Application.Interfaces
{
    public interface IJwtGenerator
    {
        string CreateToken(AppUser user);
        string GenerateRefreshToken();
    }
}
