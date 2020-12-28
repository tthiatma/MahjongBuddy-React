using AutoMapper;
using MahjongBuddy.Application.Dtos;
using MahjongBuddy.Application.Errors;
using MahjongBuddy.Core.Enums;
using MahjongBuddy.EntityFramework.EntityFramework;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace MahjongBuddy.Application.Games
{
    public class Leave
    {
        public class Command : IRequest<PlayerDto>
        {
            public string GameCode { get; set; }
            public string UserName { get; set; }
            public string ConnectionId { get; set; }
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
                var game = await _context.Games.SingleOrDefaultAsync(g => g.Code == request.GameCode);

                if (game == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Game = "Could not find game" });

                var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == request.UserName);

                var gamePlayer = await _context.GamePlayers.SingleOrDefaultAsync(x => x.GameId == game.Id && x.PlayerId == user.Id);

                if (gamePlayer == null)
                    throw new RestException(HttpStatusCode.BadRequest, new { Connect = "Player already left the game" });


                if(game.Status == GameStatus.Created)
                {
                    _context.Connections.RemoveRange(gamePlayer.Connections);
                    _context.GamePlayers.Remove(gamePlayer);
                }

                var currentConnection = gamePlayer.Connections.FirstOrDefault(c => c.Id == request.ConnectionId);
                if (currentConnection != null && game.Status == GameStatus.Playing)
                {
                    currentConnection.Connected = false;
                }

                try
                {
                    await _context.SaveChangesAsync();
                    return _mapper.Map<PlayerDto>(gamePlayer);
                }
                catch (Exception)
                {
                    throw new Exception("Problem leaving from game");
                }
            }
        }
    }
}
