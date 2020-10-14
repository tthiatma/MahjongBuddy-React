using AutoMapper;
using MahjongBuddy.Application.Dtos;
using MahjongBuddy.Application.Errors;
using MahjongBuddy.EntityFramework.EntityFramework;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace MahjongBuddy.Application.Games
{
    public class Leave
    {
        public class Command : IRequest<PlayerDto>
        {
            public int GameId { get; set; }
            public string UserName { get; set; }
        }

        public class Handler : IRequestHandler<Command, PlayerDto>
        {
            private readonly MahjongBuddyDbContext _context;
            private readonly IMapper _mapper;

            public Handler(MahjongBuddyDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<PlayerDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var game = await _context.Games.FindAsync(request.GameId);

                if (game == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Game = "Could not find game" });

                var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == request.UserName);

                var playerInGame = await _context.GamePlayers.SingleOrDefaultAsync(x => x.GameId == game.Id && x.AppUserId == user.Id);

                if (playerInGame == null)
                    throw new RestException(HttpStatusCode.BadRequest, new { Connect = "Player already left the game" });

                _context.Connections.RemoveRange(playerInGame.Connections);

                _context.GamePlayers.Remove(playerInGame);

                var success = await _context.SaveChangesAsync() > 0;

                if (success) return _mapper.Map<PlayerDto>(playerInGame);

                throw new Exception("Problem leaving from game");
            }
        }
    }
}
