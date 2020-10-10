using MahjongBuddy.EntityFramework.EntityFramework;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MahjongBuddy.Application.Hub
{
    public class OnConnected
    {
        public class Query: IRequest
        {
            public string ConnectionId { get; set; }
            public string UserAgent { get; set; }
        }

        public class Handler : IRequestHandler<Query>
        {
            private readonly MahjongBuddyDbContext _context;

            public Handler(MahjongBuddyDbContext context)
            {
                _context = context;
            }
            public async Task<Unit> Handle(Query request, CancellationToken cancellationToken)
            {
                var userGames = await _context.GamePlayers.SingleOrDefaultAsync(x => x.Connections.Any(c => c.Id == request.ConnectionId));

                //TODO find the existing connection and mark it as connected again
                return Unit.Value;
            }
        }
    }
}
