﻿using AutoMapper;
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

                var usersInGame = game.UserGames.Count;
                if (usersInGame == 4)
                    throw new RestException(HttpStatusCode.BadRequest, new { Game = "Reached max players" });

                var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == request.UserName);

                var playerInGame = await _context.UserGames.SingleOrDefaultAsync(x => x.GameId == game.Id && x.AppUserId == user.Id);

                if (playerInGame != null)
                    throw new RestException(HttpStatusCode.BadRequest, new { Connect = "Player already in the game" });

                playerInGame = new UserGame
                {
                    Game = game,
                    AppUser = user,
                    IsHost = false,
                };

                _context.UserGames.Add(playerInGame);

                var success = await _context.SaveChangesAsync() > 0;

                if (success) return _mapper.Map<PlayerDto>(playerInGame);

                throw new Exception("Problem joining to game");
            }
        }
    }
}