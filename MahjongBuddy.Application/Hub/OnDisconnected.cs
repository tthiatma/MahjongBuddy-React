using MahjongBuddy.EntityFramework.EntityFramework;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace MahjongBuddy.Application.Hub
{
    public class OnDisConnected
    {
        public class Query : IRequest
        {
            public string ConnectionId { get; set; }
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
                var connection = await _context.Connections.SingleOrDefaultAsync(x => x.Id == request.ConnectionId);

                if (connection != null)
                {
                    if (connection.Connected)
                    {
                        connection.Connected = false;
                        _context.SaveChanges();
                    }
                }
                return Unit.Value;
            }
        }
    }
}
