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
                    Email = "tonny@gmail.com"
                };
                var topPlayer = new AppUser
                {
                    Id = "c",
                    DisplayName = "Peter",
                    UserName = "Peter",
                    Email = "peter@gmail.com"
                };
                var leftPlayer = new AppUser
                {
                    Id = "b",
                    DisplayName = "Mei",
                    UserName = "Mei",
                    Email = "mei@gmail.com"
                };
                var rightPlayer = new AppUser
                {
                    Id = "d",
                    DisplayName = "Jason",
                    UserName = "Jason",
                    Email = "jason@gmail.com"
                };

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
