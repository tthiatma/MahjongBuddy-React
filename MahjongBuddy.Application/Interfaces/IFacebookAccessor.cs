using MahjongBuddy.Application.User;
using System.Threading.Tasks;

namespace MahjongBuddy.Application.Interfaces
{
    public interface IFacebookAccessor
    {
        Task<FacebookUserInfo> FacebookLogin(string accessToken);
    }
}
