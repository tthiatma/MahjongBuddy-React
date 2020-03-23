using MahjongBuddy.Application.Errors;
using MahjongBuddy.Core;
using MahjongBuddy.EntityFramework.EntityFramework;
using MediatR;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
namespace MahjongBuddy.Application.Games
{
    public class Detail
    {
        public class Query : IRequest<Game>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Game>
        {
            private readonly MahjongBuddyDbContext _context;

            public Handler(MahjongBuddyDbContext context)
            {
                _context = context;
            }

            public async Task<Game> Handle(Query request, CancellationToken cancellationToken)
            {
                var game = await _context.Games.FindAsync(request.Id);

                if (game == null)
                    throw new RestException(HttpStatusCode.NotFound, new { game = "Not Found" });

                return game;
            }
        }
    }
}
