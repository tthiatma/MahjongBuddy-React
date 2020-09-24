using MahjongBuddy.Core;
using MahjongBuddy.Core.Enums;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace MahjongBuddy.EntityFramework.EntityFramework
{
    public class MahjongBuddyDbContext : IdentityDbContext<AppUser>
    {
        //dotnet ef migrations add "InitialCreate" -p MahjongBuddy.EntityFramework/ -s MahjongBuddy.API/

        //migrationBuilder.Sql(
        //        @"
        //        CREATE TRIGGER SetRoundTileTimestampOnUpdate
        //        AFTER UPDATE ON RoundTiles
        //        BEGIN
        //            UPDATE RoundTiles
        //            SET Timestamp = randomblob(8)
        //            WHERE rowid = NEW.rowid;
        //        END
        //        ");

        //    migrationBuilder.Sql(
        //                @"
        //        CREATE TRIGGER SetRoundTileTimestampOnInsert
        //        AFTER INSERT ON RoundTiles
        //        BEGIN
        //            UPDATE RoundTiles
        //            SET Timestamp = randomblob(8)
        //            WHERE rowid = NEW.rowid;
        //        END
        //    ");

        //migrationBuilder.Sql(
        //        @"
        //        CREATE TRIGGER SetRoundPlayerTimestampOnUpdate
        //        AFTER UPDATE ON RoundPlayers
        //        BEGIN
        //            UPDATE RoundPlayers
        //            SET Timestamp = randomblob(8)
        //            WHERE rowid = NEW.rowid;
        //        END
        //        ");

        //    migrationBuilder.Sql(
        //                @"
        //        CREATE TRIGGER SetRoundPlayerTimestampOnInsert
        //        AFTER INSERT ON RoundPlayers
        //        BEGIN
        //            UPDATE RoundPlayers
        //            SET Timestamp = randomblob(8)
        //            WHERE rowid = NEW.rowid;
        //        END
        //    ");

        public MahjongBuddyDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<ChatMsg> ChatMsgs { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<RoundTile> RoundTiles { get; set; }
        public DbSet<Round> Rounds { get; set; }
        public DbSet<RoundResult> RoundResults { get; set; }
        public DbSet<Tile> Tiles { get; set; }
        public DbSet<GamePlayer> GamePlayers { get; set; }
        public DbSet<RoundPlayer> RoundPlayers { get; set; }
        public DbSet<RoundResultHand> RoundHands { get; set; }
        public DbSet<RoundResultExtraPoint> RoundExtraPoints { get; set; }
        public DbSet<Photo> Photos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            ConvertEnum(builder);

            builder.Entity<GamePlayer>(x => x.HasKey(gp => new { gp.GameId, gp.AppUserId }));
            builder.Entity<GamePlayer>()
                .HasOne(u => u.AppUser)
                .WithMany(g => g.UserGames)
                .HasForeignKey(u => u.AppUserId);

            builder.Entity<GamePlayer>()
                .HasOne(g => g.Game)
                .WithMany(u => u.GamePlayers)
                .HasForeignKey(g => g.GameId);

            builder.Entity<RoundTile>().Property(x => x.Timestamp)
            .IsConcurrencyToken(true)
            .ValueGeneratedOnAddOrUpdate();

            builder.Entity<RoundPlayer>(x => x.HasKey(ur => new { ur.RoundId, ur.AppUserId }));
            builder.Entity<RoundPlayer>()
                .HasOne(u => u.AppUser)
                .WithMany(r => r.UserRounds)
                .HasForeignKey(u => u.AppUserId);

            builder.Entity<RoundPlayer>()
                .HasOne(r => r.Round)
                .WithMany(u => u.RoundPlayers)
                .HasForeignKey(r => r.RoundId);
        }

        private void ConvertEnum(ModelBuilder builder)
        {
            builder.Entity<Game>()
                .Property(e => e.Status)
                .HasConversion(
                v => v.ToString(),
                v => (GameStatus)Enum.Parse(typeof(GameStatus), v));

            builder.Entity<Round>()
                .Property(e => e.Wind)
                .HasConversion(
                v => v.ToString(),
                v => (WindDirection)Enum.Parse(typeof(WindDirection), v));

            builder.Entity<RoundPlayer>()
                .Property(e => e.Wind)
                .HasConversion(
                v => v.ToString(),
                v => (WindDirection)Enum.Parse(typeof(WindDirection), v));

            builder.Entity<RoundTile>()
                .Property(e => e.TileSetGroup)
                .HasConversion(
                v => v.ToString(),
                v => (TileSetGroup)Enum.Parse(typeof(TileSetGroup), v));

            builder.Entity<RoundTile>()
                .Property(e => e.Status)
                .HasConversion(
                v => v.ToString(),
                v => (TileStatus)Enum.Parse(typeof(TileStatus), v));

            builder.Entity<Tile>()
                .Property(e => e.TileType)
                .HasConversion(
                v => v.ToString(),
                v => (TileType)Enum.Parse(typeof(TileType), v));

            builder.Entity<Tile>()
                .Property(e => e.TileValue)
                .HasConversion(
                v => v.ToString(),
                v => (TileValue)Enum.Parse(typeof(TileValue), v));

            builder.Entity<RoundResultHand>()
                .Property(e => e.HandType)
                .HasConversion(
                v => v.ToString(),
                v => (HandType)Enum.Parse(typeof(HandType), v));

            builder.Entity<RoundResultExtraPoint>()
                .Property(e => e.ExtraPoint)
                .HasConversion(
                v => v.ToString(),
                v => (ExtraPoint)Enum.Parse(typeof(ExtraPoint), v));

            builder.Entity<RoundPlayerAction>()
                .Property(e => e.PlayerAction)
                .HasConversion(
                v => v.ToString(),
                v => (ActionType)Enum.Parse(typeof(ActionType), v));
        }
    }
}
