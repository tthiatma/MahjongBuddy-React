using MahjongBuddy.EntityFramework.EntityFramework;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MahjongBuddy.Application.Games
{
    public class Edit
    {
        public class Command : IRequest
        {
            public int Id { get; set; }

            public string Title { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly MahjongBuddyDbContext _context;

            public Handler(MahjongBuddyDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                //handler logic
                var game = await _context.Games.FindAsync(request.Id);

                if (game == null)
                    throw new Exception("Could not find game");

                game.Title = request.Title ?? game.Title;

                var success = await _context.SaveChangesAsync() > 0;

                if (success) return Unit.Value;

                throw new Exception("Problem saving changes");
            }
        }
    }
}
