using AutoMapper;
using MahjongBuddy.Application.Dtos;
using MahjongBuddy.Application.Errors;
using MahjongBuddy.Application.Interfaces;
using MahjongBuddy.Core;
using MahjongBuddy.Core.Enums;
using MahjongBuddy.EntityFramework.EntityFramework;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace MahjongBuddy.Application.Games
{
    public class Create
    {
        public class Command : IRequest<GameDto>
        {
            public string Title { get; set; }
            public string MinPoint { get; set; }
            public string MaxPoint { get; set; }
        }
        public class Handler : IRequestHandler<Command, GameDto>
        {
            private readonly MahjongBuddyDbContext _context;
            private readonly IUserAccessor _userAccessor;
            private readonly IMapper _mapper;
            private readonly UserManager<Player> _userManager;
            private readonly IGameCodeGenerator _gameCodeGenerator;

            public Handler(MahjongBuddyDbContext context
                , IUserAccessor userAccessor
                , IMapper mapper
                , UserManager<Player> userManager
                ,IGameCodeGenerator gameCodeGenerator)
            {
                _context = context;
                _userAccessor = userAccessor;
                _mapper = mapper;
                _userManager = userManager;
                _gameCodeGenerator = gameCodeGenerator;
            }

            public async Task<GameDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == _userAccessor.GetCurrentUserName());

                //check if user spamming create game, if there is more than 10 games created within one minutes return 401 and lock user out lol
                var gamesWithinOneMin = _context.Games.Where(g => g.HostId == user.Id && g.Date > DateTime.Now.AddMinutes(-1));

                if(gamesWithinOneMin != null && gamesWithinOneMin.Count() > 10)
                {
                    user.LockoutEnd = DateTime.Now.AddMonths(1);
                    foreach (var rt in user.RefreshTokens)
                    {
                        rt.Revoked = DateTime.Now;
                    }
                    await _userManager.UpdateAsync(user);
                    throw new RestException(HttpStatusCode.Unauthorized);
                }

                var gameCode = _gameCodeGenerator.CreateCode();

                var minPoint = int.Parse(request.MinPoint);
                var maxPoint = int.Parse(request.MaxPoint);
                //TODO don't hard code min and maxpoint
                var game = new Game
                {
                    Title = request.Title,
                    Code = _gameCodeGenerator.CreateCode(),
                    Date = DateTime.Now,
                    Host = user,
                    Status = GameStatus.Created,
                    MinPoint = minPoint,
                    MaxPoint = maxPoint == 0 ? 10 : maxPoint,
                    GamePlayers = new List<GamePlayer>{
                        new GamePlayer {
                            IsHost = true,
                            Player = user
                        }
                    }
                };

                _context.Games.Add(game);

                var success = await _context.SaveChangesAsync() > 0;

                var gameToReturn = _mapper.Map<Game, GameDto>(game);

                if (success) return gameToReturn;

                throw new Exception("Problem saving changes");
            }
        }
    }
}
