using AutoMapper;
using MahjongBuddy.Application.Dtos;
using MahjongBuddy.Application.Errors;
using MahjongBuddy.Core;
using MahjongBuddy.EntityFramework.EntityFramework;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace MahjongBuddy.Application.Games
{
    public class Join
    {
        public class Command : IRequest<PlayerDto>
        {
            public int GameId { get; set; }
            public string UserName { get; set; }
            public string ConnectionId { get; set; }
            public string UserAgent { get; set; }
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

                var gamePlayer = await _context.GamePlayers.SingleOrDefaultAsync(x => x.GameId == game.Id && x.PlayerId == user.Id);

                if (gamePlayer != null)
                {
                    //set connection to connected if its already exist
                    var existingConnection = await _context.Connections.FirstOrDefaultAsync(c => c.Id == request.ConnectionId);
                    if (existingConnection != null)
                    {
                        existingConnection.Connected = true;
                        existingConnection.UserAgent = request.UserAgent;
                        existingConnection.GamePlayerId = gamePlayer.Id;
                    }
                }
                else
                {
                    var usersInGame = game.GamePlayers.Count;
                    if (usersInGame == 4)
                        throw new RestException(HttpStatusCode.BadRequest, new { Game = "Reached max players" });

                    gamePlayer = new GamePlayer
                    {
                        Game = game,
                        Player = user,
                        IsHost = false,
                    };
                    gamePlayer.Connections.Add(new Connection { 
                        Id = request.ConnectionId, 
                        Connected = true, 
                        UserAgent = request.UserAgent });
                    _context.GamePlayers.Add(gamePlayer);
                }

                var success = await _context.SaveChangesAsync() > 0;

                if (success) return _mapper.Map<PlayerDto>(gamePlayer);

                throw new Exception("Problem joining to game");
            }
        }
    }
}
