﻿using AutoMapper;
using MahjongBuddy.Application.Interfaces;
using MahjongBuddy.Core;
using MahjongBuddy.EntityFramework.EntityFramework;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
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
                var game = new Game
                {
                    Title = request.Title,
                    Date = DateTime.Now                    
                };

                _context.Games.Add(game);

                var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == _userAccessor.GetCurrentUserName());

                var player = new UserGame
                {
                    AppUser = user,
                    Game = game,
                    IsHost = true,
                };

                _context.UserGames.Add(player);

                var success = await _context.SaveChangesAsync() > 0;

                var gameToReturn = _mapper.Map<Game, GameDto>(game);
                if (success) return gameToReturn;

                throw new Exception("Problem saving changes");
            }
        }
    }
}
