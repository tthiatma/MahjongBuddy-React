using MahjongBuddy.Core;
using MahjongBuddy.EntityFramework.EntityFramework;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MahjongBuddy.Application.PlayerAction
{
    public class List
    {
        public class Query : IRequest<List<Tile>> { }

        public class Handler : IRequestHandler<Query, List<Tile>>
        {
            private readonly MahjongBuddyDbContext _context;

            public Handler(MahjongBuddyDbContext context)
            {
                _context = context;
            }

            public async Task<List<Tile>> Handle(Query request, CancellationToken cancellationToken)
            {
                var activities = await _context.Tiles.ToListAsync();

                return activities;
            }
        }
    }
}
