using MahjongBuddy.Core;
using MahjongBuddy.EntityFramework.EntityFramework;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MahjongBuddy.Application.Games
{
    public class List
    {
        public class Query : IRequest<List<Game>> { }

        public class Handler : IRequestHandler<Query, List<Game>>
        {
            private readonly MahjongBuddyDbContext _context;

            public Handler(MahjongBuddyDbContext context)
            {
                _context = context;
            }

            public async Task<List<Game>> Handle(Query request, CancellationToken cancellationToken)
            {
                var games = await _context.Games.Include(g=> g.GameTiles).ThenInclude(gt => gt.Tile).ToListAsync();

                return games;
            }
        }
    }
}
