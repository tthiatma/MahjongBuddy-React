using AutoMapper;
using MahjongBuddy.Application.Dtos;
using MahjongBuddy.Application.Errors;
using MahjongBuddy.Core;
using MahjongBuddy.Core.Enums;
using MahjongBuddy.EntityFramework.EntityFramework;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Linq;
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

                var playerInGame = game.GamePlayers.Any(p => p.Player.UserName == request.UserName);
                if (!playerInGame)
                {
                    if (game.Status == GameStatus.Playing || game.GamePlayers.Count == 4)
                    {
                        throw new RestException(HttpStatusCode.BadRequest, new { Game = "Reached max players" });
                    }
                }

                if (gamePlayer != null)
                {
                    foreach (var uc in gamePlayer.Connections)
                    {
                        if (uc.Id != request.ConnectionId)
                        {
                            _context.Connections.Remove(uc);
                        }
                        else
                        {
                            uc.Connected = true;
                        }
                    }
                    var existingConnection = _context.Connections.FirstOrDefault(c => c.Id == request.ConnectionId);
                    if (existingConnection == null)
                    {
                        gamePlayer.Connections.Add(new Connection
                        {
                            Id = request.ConnectionId,
                            GamePlayerId = gamePlayer.Id,
                            Connected = true,
                            UserAgent = request.UserAgent
                        });
                    }
                }
                else
                {
                    gamePlayer = new GamePlayer
                    {
                        Game = game,
                        Player = user,
                        IsHost = false,
                    };
                    gamePlayer.Connections.Add(new Connection
                    {
                        Id = request.ConnectionId,
                        Connected = true,
                        UserAgent = request.UserAgent
                    });
                    _context.GamePlayers.Add(gamePlayer);
                }

                try
                {
                    await _context.SaveChangesAsync();
                    return _mapper.Map<PlayerDto>(gamePlayer);
                }
                catch (Exception)
                {
                    throw new Exception("Problem joining to game");
                }
            }
        }
    }
}
