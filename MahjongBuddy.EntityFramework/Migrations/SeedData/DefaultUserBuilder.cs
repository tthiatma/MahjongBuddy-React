using MahjongBuddy.Core;
using MahjongBuddy.EntityFramework.EntityFramework;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MahjongBuddy.EntityFramework.Migrations.SeedData
{
    public class DefaultUserBuilder
    {
        private readonly MahjongBuddyDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        public DefaultUserBuilder(MahjongBuddyDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task Build()
        {
            if (!_context.Users.Any())
            {
                var mainPlayer = new AppUser
                {
                    Id = "a",
                    DisplayName = "Tonny",
                    UserName = "Tonny",
                    Email = "tonny@gmail.com",
                    EmailConfirmed = true
                };
                mainPlayer.Photos.Add(new Photo
                {
                    Id = "ps9p70gkirosjaudenvl",
                    Url = "https://res.cloudinary.com/mahjongbuddy/image/upload/v1598673581/ps9p70gkirosjaudenvl.jpg",
                    IsMain = true
                });

                var rightPlayer = new AppUser
                {
                    Id = "b",
                    DisplayName = "Mei",
                    UserName = "Mei",
                    Email = "mei@gmail.com",
                    EmailConfirmed = true
                };
                rightPlayer.Photos.Add(new Photo
                {
                    Id = "akrhqqhnjsrsnycub48x",
                    Url = "https://res.cloudinary.com/mahjongbuddy/image/upload/v1598673644/akrhqqhnjsrsnycub48x.jpg",
                    IsMain = true
                });

                var topPlayer = new AppUser
                {
                    Id = "c",
                    DisplayName = "Peter",
                    UserName = "Peter",
                    Email = "peter@gmail.com",
                    EmailConfirmed = true
                };
                topPlayer.Photos.Add(new Photo
                {
                    Id = "qzqes8q87s5q0acs0tos",
                    Url = "https://res.cloudinary.com/mahjongbuddy/image/upload/v1598673707/qzqes8q87s5q0acs0tos.jpg",
                    IsMain = true
                });

                var leftPlayer = new AppUser
                {
                    Id = "d",
                    DisplayName = "Jason",
                    UserName = "Jason",
                    Email = "jason@gmail.com",
                    EmailConfirmed = true
                };
                leftPlayer.Photos.Add(new Photo
                {
                    Id = "u6st5f6gbr9enrxbeeqa",
                    Url = "https://res.cloudinary.com/mahjongbuddy/image/upload/v1598673746/u6st5f6gbr9enrxbeeqa.jpg",
                    IsMain = true
                });

                var users = new List<AppUser>
                {
                    mainPlayer,
                    leftPlayer,
                    topPlayer,
                    rightPlayer
                };

                foreach (var user in users)
                {
                    await _userManager.CreateAsync(user, "Pa$$w0rd");
                }
                _context.SaveChanges();
            }
        }
    }
}
