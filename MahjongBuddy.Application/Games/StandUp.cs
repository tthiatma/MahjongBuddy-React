using AutoMapper;
using MahjongBuddy.Application.Dtos;
using MahjongBuddy.Application.Errors;
using MahjongBuddy.Core.Enums;
using MahjongBuddy.EntityFramework.EntityFramework;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace MahjongBuddy.Application.Games
{
    public class StandUp
    {
        public class Command : IRequest<GamePlayerDto>
        {
            public string GameCode { get; set; }
            public string UserName { get; set; }
        }

        public class Handler : IRequestHandler<Command, GamePlayerDto>
        {
            private readonly MahjongBuddyDbContext _context;
            private readonly IMapper _mapper;

            public Handler(MahjongBuddyDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<GamePlayerDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var game = await _context.Games.FirstOrDefaultAsync(g => g.Code == request.GameCode);

                if (game == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Game = "Could not find game" });

                if (game.Status == GameStatus.Playing || game.Status == GameStatus.Over)
                    throw new RestException(HttpStatusCode.NotFound, new { Game = "Can't stand up to a game that's already started/over" });

                var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == request.UserName);

                var playerInGame = await _context.GamePlayers.SingleOrDefaultAsync(x => x.GameId == game.Id && x.PlayerId == user.Id);

                if (playerInGame == null)
                    throw new RestException(HttpStatusCode.BadRequest, new { Connect = "Player already left the game" });

                playerInGame.InitialSeatWind = null;

                try
                {
                    await _context.SaveChangesAsync();
                    return _mapper.Map<GamePlayerDto>(playerInGame);
                }
                catch (Exception)
                {
                    throw new Exception("Problem standing up from game");
                }
            }
        }
    }
}
