﻿using MahjongBuddy.Application.Interfaces;
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
        public class Command : IRequest
        {
            public int Id { get; set; }

            public string Title { get; set; }

        }
        public class Handler : IRequestHandler<Command> 
        {
            private readonly MahjongBuddyDbContext _context;
            private readonly IUserAccessor _userAccessor;

            public Handler(MahjongBuddyDbContext context, IUserAccessor userAccessor)
            {
                _context = context;
                _userAccessor = userAccessor;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var game = new Game
                {
                    Id = request.Id,
                    Title = request.Title
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

                if (success) return Unit.Value;

                throw new Exception("Problem saving changes");
            }
        }
    }
}
