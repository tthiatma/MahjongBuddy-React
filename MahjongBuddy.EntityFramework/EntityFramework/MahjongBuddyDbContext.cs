using MahjongBuddy.Core;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MahjongBuddy.EntityFramework.EntityFramework
{
    public class MahjongBuddyDbContext : IdentityDbContext<AppUser>
    {
        //dotnet ef migrations add "InitialCreate" -p MahjongBuddy.EntityFramework/ -s MahjongBuddy.API/
        //dotnet ef migrations add "AddedIdentity" -p MahjongBuddy.EntityFramework/ -s MahjongBuddy.API/
        public MahjongBuddyDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Game> Games { get; set; }

        public DbSet<Tile> Tiles { get; set; }

        public DbSet<GameTile> GameTiles { get; set; }

        public DbSet<UserGame> UserGames { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserGame>(x => x.HasKey(ug => new { ug.GameId, ug.AppUserId }));
            builder.Entity<UserGame>()
                .HasOne(u => u.AppUser)
                .WithMany(g => g.UserGames)
                .HasForeignKey(u => u.AppUserId);
            builder.Entity<UserGame>()
                .HasOne(g => g.Game)
                .WithMany(u => u.UserGames)
                .HasForeignKey(g => g.GameId);

        }
    }
}
