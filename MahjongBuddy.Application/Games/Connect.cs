using MahjongBuddy.Application.Errors;
using MahjongBuddy.Application.Interfaces;
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
    public class Connect : IRequest
    {
        public class Command : IRequest
        {
            public int Id { get; set; }

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
                var game = await _context.Games.FindAsync(request.Id);

                if (game == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Game = "Could not find game" });

                var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == _userAccessor.GetCurrentUserName());

                var connected = await _context.UserGames.SingleOrDefaultAsync(x => x.GameId == game.Id && x.AppUserId == user.Id);

                if(connected != null)
                    throw new RestException(HttpStatusCode.BadRequest, new { Connect = "Player already in the game" });

                connected = new UserGame
                {
                    Game = game,
                    AppUser = user,
                    IsHost = false
                };

                _context.UserGames.Add(connected);

                var success = await _context.SaveChangesAsync() > 0;

                if (success) return Unit.Value;

                throw new Exception("Problem saving changes");
            }
        }
    }
}
