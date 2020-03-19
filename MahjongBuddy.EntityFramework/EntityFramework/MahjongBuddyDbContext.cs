using MahjongBuddy.Core;
using Microsoft.EntityFrameworkCore;

namespace MahjongBuddy.EntityFramework.EntityFramework
{
    public class MahjongBuddyDbContext : DbContext
    {
        public MahjongBuddyDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Tile> Tiles { get; set; }

        public DbSet<Game> Games { get; set; }
        
    }
}
