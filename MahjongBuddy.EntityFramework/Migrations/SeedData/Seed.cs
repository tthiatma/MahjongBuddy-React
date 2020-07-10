using MahjongBuddy.Core;
using MahjongBuddy.EntityFramework.Migrations.SeedData;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace MahjongBuddy.EntityFramework.EntityFramework
{
    public class Seed
    {
        public static async Task SeedData(MahjongBuddyDbContext context, UserManager<AppUser> userManager)
        {
            var userBuilder = new DefaultUserBuilder(context, userManager);
            await userBuilder.Build();
            new DefaultTileBuilder(context).Build();
            new DefaultGameBuilder(context).Build();
        }
    }
}
