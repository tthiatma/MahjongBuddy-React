using MahjongBuddy.Core;

namespace MahjongBuddy.Application.Interfaces
{
    public interface IJwtGenerator
    {
        string CreateToken(Player user);
        RefreshToken GenerateRefreshToken();
    }
}
