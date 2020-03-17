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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Tile>()
                .HasData(
                    new Tile { Id = 1, Name = "Tile 1"},
                    new Tile { Id = 2, Name = "Tile 2" }
                );
        }
    }
}
