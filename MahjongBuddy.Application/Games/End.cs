using MahjongBuddy.Application.Dtos;
using MediatR;
using AutoMapper;
using MahjongBuddy.Application.Interfaces;
using MahjongBuddy.EntityFramework.EntityFramework;
using System;
using System.Threading;
using System.Threading.Tasks;
using MahjongBuddy.Core;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MahjongBuddy.Core.Enums;

namespace MahjongBuddy.Application.Games
{
    public class End
    {
        public class Command: IRequest<GameDto>
        {
            public int GameId { get; set; }
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
                var game = await _context.Games.SingleOrDefaultAsync(x => x.Id == request.GameId);
                if(game == null)
                    throw new Exception("Could not find game");

                var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == _userAccessor.GetCurrentUserName());
                if (user == null)
                    throw new Exception("Could not find user");

                var hostPlayer = game.GamePlayers.FirstOrDefault(p => p.PlayerId == user.Id && p.IsHost == true);
                if(hostPlayer == null)
                    throw new Exception("Only host can end the game");

                var hasUnfinishedRound = game.Rounds.Any(r => !r.IsOver);

                game.Status = hasUnfinishedRound ? GameStatus.OverPrematurely : GameStatus.Over;

                var success = await _context.SaveChangesAsync() > 0;

                var gameToReturn = _mapper.Map<Game, GameDto>(game);

                if (success) return gameToReturn;

                throw new Exception("Problem saving changes");
            }
        }
    }
}
