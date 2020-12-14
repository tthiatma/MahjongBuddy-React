using AutoMapper;
using MahjongBuddy.Application.Games;
using MahjongBuddy.Application.Interfaces;
using MahjongBuddy.Core;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Threading;
using Xunit;

namespace MahjongBuddy.Application.Tests.Games
{
    public class CreateTest: TestBase
    {
        private readonly IMapper _mapper;

        public CreateTest()
        {
            var mockMapper = new MapperConfiguration(cfg => { cfg.AddProfile(new MappingProfile()); });
            _mapper = mockMapper.CreateMapper();
        }

        [Fact]
        public void Should_Create_Game()
        {
            var userAccessor = new Mock<IUserAccessor>();
            var userManager = new Mock<UserManager<Player>>();
            userAccessor.Setup(u => u.GetCurrentUserName()).Returns("mainplayer");

            var context = GetDbContext();

            context.Users.AddAsync(new Player
            {
                Id = "a",
                Email = "mainplayer@gmail.com.com",
                UserName = "mainplayer"
            });
            context.SaveChangesAsync();

            var gameCommand = new Create.Command
            {
                Title = "Game1",
                MinPoint = "3",
                MaxPoint = "10"
            };

            var sut = new Create.Handler(context, userAccessor.Object, _mapper, userManager.Object);
            var result = sut.Handle(gameCommand, CancellationToken.None).Result;

            Assert.Equal("Game1", result.Title);
        }
    }
}
