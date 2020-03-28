using MahjongBuddy.Core.AppUsers;
using MahjongBuddy.EntityFramework.Migrations.SeedData;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MahjongBuddy.EntityFramework.EntityFramework
{
    public class Seed
    {
        public static async Task SeedData(MahjongBuddyDbContext context, UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var users = new List<AppUser>
                {
                    new AppUser
                    {
                        Id="a",
                        DisplayName = "MainPlayer",
                        UserName = "MainPlayer",
                        Email = "mainplayer@gmail.com"

                    },
                    new AppUser
                    {
                        Id="b",
                        DisplayName = "LeftPlayer",
                        UserName = "LeftPlayer",
                        Email = "leftplayergmail.com"

                    },
                    new AppUser
                    {
                        Id="c",
                        DisplayName = "TopPlayer",
                        UserName = "TopPlayer",
                        Email = "topplayer@gmail.com"

                    },
                    new AppUser
                    {
                        Id="d",
                        DisplayName = "RightPlayer",
                        UserName = "RightPlayer",
                        Email = "rightplayer@gmail.com"

                    }
                };

                foreach (var user in users)
                {
                   await userManager.CreateAsync(user, "Pa$$w0rd");
                }
                context.SaveChanges();

                new DefaultGameBuilder(context).Build();
                new DefaultTileBuilder(context).Build();
                new DefaulrGameTileBuilder(context).Build();
            }
        }
    }
}
