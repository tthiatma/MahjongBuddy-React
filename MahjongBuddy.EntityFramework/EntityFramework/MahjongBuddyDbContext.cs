using MahjongBuddy.Core;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MahjongBuddy.EntityFramework.EntityFramework
{
    public class MahjongBuddyDbContext : IdentityDbContext<AppUser>
    {
        //dotnet ef migrations add "InitialCreate" -p MahjongBuddy.EntityFramework/ -s MahjongBuddy.API/
        public MahjongBuddyDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<ChatMsg> ChatMsgs { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<RoundTile> RoundTiles { get; set; }
        public DbSet<Round> Rounds { get; set; }
        public DbSet<RoundResult> RoundResults { get; set; }
        public DbSet<Tile> Tiles { get; set; }
        public DbSet<UserGame> UserGames { get; set; }
        public DbSet<UserRound> UserRounds { get; set; }
        public DbSet<RoundResultHand> RoundHands { get; set; }
        public DbSet<RoundResultExtraPoint> RoundExtraPoints { get; set; }

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

            builder.Entity<UserRound>(x => x.HasKey(ur => new { ur.RoundId, ur.AppUserId }));
            builder.Entity<UserRound>()
                .HasOne(u => u.AppUser)
                .WithMany(r => r.UserRounds)
                .HasForeignKey(u => u.AppUserId);
            builder.Entity<UserRound>()
                .HasOne(r => r.Round)
                .WithMany(u => u.UserRounds)
                .HasForeignKey(r => r.RoundId);
        }
    }
}
