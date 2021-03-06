﻿using AutoMapper;
using MahjongBuddy.Application.Dtos;
using MahjongBuddy.Application.Errors;
using MahjongBuddy.Core;
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
    public class Sit
    {
        public class Command : IRequest<GamePlayerDto>
        {
            public string GameCode { get; set; }
            public string UserName { get; set; }
            public WindDirection InitialSeatWind { get; set; }

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
                var game = await _context.Games.FirstOrDefaultAsync(g => g.Code == request.GameCode.ToUpper());

                if (game == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Game = "Could not find game" });

                if (game.Status == GameStatus.Playing || game.Status == GameStatus.Over)
                    throw new RestException(HttpStatusCode.NotFound, new { Game = "Can't sit on game that's already started/over" });

                var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == request.UserName);

                var playerInGame = await _context.GamePlayers.SingleOrDefaultAsync(x => x.GameId == game.Id && x.PlayerId == user.Id);

                if(playerInGame == null)
                    throw new RestException(HttpStatusCode.BadRequest, new { Connect = "Player not in the game" });

                playerInGame.InitialSeatWind = request.InitialSeatWind;

                var success = await _context.SaveChangesAsync() > 0;

                if (success) return _mapper.Map<GamePlayerDto>(playerInGame);

                throw new Exception("Problem sitting to game");
            }
        }
    }
}
