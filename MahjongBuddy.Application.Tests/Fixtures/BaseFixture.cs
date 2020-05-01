using System;
using AutoMapper;
using MahjongBuddy.EntityFramework.EntityFramework;
using MahjongBuddy.EntityFramework.Migrations.SeedData;
using Microsoft.EntityFrameworkCore;

namespace MahjongBuddy.Application.Tests.Fixtures
{
    public class BaseFixture : IDisposable
    {
        public IMapper GameMapper { get; set; }
        public IMapper RoundMapper { get; set; }
        public MahjongBuddyDbContext TestDataContext { get; set; }

        public BaseFixture()
        {
            var builder = new DbContextOptionsBuilder<MahjongBuddyDbContext>();
            builder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            TestDataContext = new MahjongBuddyDbContext(builder.Options);
            TestDataContext.Database.EnsureCreated();

            new DefaultTileBuilder(TestDataContext).Build();

            var mockGameMapper = new MapperConfiguration(cfg => { cfg.AddProfile(new Application.Games.MappingProfile()); });
            GameMapper = mockGameMapper.CreateMapper();

            var mockRoundMapper = new MapperConfiguration(cfg => { cfg.AddProfile(new Application.Rounds.MappingProfile()); });
            RoundMapper = mockRoundMapper.CreateMapper();

        }

        public virtual void Dispose()
        {
            TestDataContext.Dispose();            
        }
    }
}
