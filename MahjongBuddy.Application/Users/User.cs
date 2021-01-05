using MahjongBuddy.Application.Interfaces;
using MahjongBuddy.Core;
using System.Linq;
using System.Text.Json.Serialization;

namespace MahjongBuddy.Application.Users
{
    public class User
    {
        public User(Player user, IJwtGenerator jwtGenerator, string refreshToken)
        {
            DisplayName = user.DisplayName;
            Token = jwtGenerator.CreateToken(user);
            UserName = user.UserName;
            Image = user.Photos.FirstOrDefault(x => x.IsMain)?.Url;
            RefreshToken = refreshToken;
        }
        public string DisplayName { get; set; }

        public string Token { get; set; }

        public string UserName { get; set; }

        public string Image { get; set; }

        [JsonIgnore]
        public string RefreshToken { get; set; }
    }
}
