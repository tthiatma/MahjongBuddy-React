using MahjongBuddy.Application.Interfaces;
using MahjongBuddy.EntityFramework.EntityFramework;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MahjongBuddy.Application.Games
{
    public class Delete
    {
        public class Command : IRequest
        {
            public int Id { get; set; }

            public string UserName { get; set; }
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
                //handler logic 
                var game = await _context.Games.FindAsync(request.Id);

                if (game == null)
                    throw new Exception("Could not find game");

                var currentUserName = string.IsNullOrEmpty(request.UserName) ? _userAccessor.GetCurrentUserName() : request.UserName;

                var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == currentUserName);

                if(user == null)
                    throw new Exception("invalid user");

                if (game.HostId != user.Id)
                    throw new Exception("only host can delete the game");

                _context.Games.Remove(game);

                var success = await _context.SaveChangesAsync() > 0;

                if (success) return Unit.Value;

                throw new Exception("Problem saving changes");
            }
        }

    }
}
