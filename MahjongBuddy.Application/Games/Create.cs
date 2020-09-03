using AutoMapper;
using MahjongBuddy.Application.Dtos;
using MahjongBuddy.Application.Interfaces;
using MahjongBuddy.Core;
using MahjongBuddy.Core.Enums;
using MahjongBuddy.EntityFramework.EntityFramework;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MahjongBuddy.Application.Games
{
    public class Create
    {
        public class Command : IRequest<GameDto>
        {
            public string Title { get; set; }

        }
        public class Handler : IRequestHandler<Command, GameDto>
        {
            private readonly MahjongBuddyDbContext _context;
            private readonly IUserAccessor _userAccessor;
            private readonly IMapper _mapper;

            public Handler(MahjongBuddyDbContext context, IUserAccessor userAccessor, IMapper mapper)
            {
                _context = context;
                _userAccessor = userAccessor;
                _mapper = mapper;
            }

            public async Task<GameDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == _userAccessor.GetCurrentUserName());

                //TODO don't hard code min and maxpoint
                var game = new Game
                {
                    Title = request.Title,
                    Date = DateTime.Now,
                    Host = user,
                    Status = GameStatus.Created,
                    MinPoint = 3,
                    MaxPoint = 10,
                    UserGames = new List<UserGame>{
                        new UserGame {
                            IsHost = true,
                            AppUser = user
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
