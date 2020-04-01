using System;
using MahjongBuddy.EntityFramework.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace MahjongBuddy.Application.Tests
{
    public class TestBase
    {
        public MahjongBuddyDbContext GetDbContext(bool useSqlite = false)
        {
            var builder = new DbContextOptionsBuilder<MahjongBuddyDbContext>();
            if (useSqlite)
            {
                builder.UseSqlite("Data Source=:memory", x => { });
            }
            else
            {
                builder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            }

            var dbContext = new MahjongBuddyDbContext(builder.Options);

            if (useSqlite)
            {
                dbContext.Database.OpenConnection();
            }

            dbContext.Database.EnsureCreated();

            return dbContext;
        }
    }
}
