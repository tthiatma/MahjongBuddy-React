using MahjongBuddy.EntityFramework.Migrations.SeedData;

namespace MahjongBuddy.EntityFramework.EntityFramework
{
    public class Seed
    {
        public static void SeedData(MahjongBuddyDbContext context)
        {
            new DefaultGameBuilder(context).Build();
            new DefaultTileBuilder(context).Build();
            new DefaulrGameTileBuilder(context).Build();

            context.SaveChanges();
        }
    }
}
