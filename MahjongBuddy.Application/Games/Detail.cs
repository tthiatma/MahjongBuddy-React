using AutoMapper;
using MahjongBuddy.Application.Dtos;
using MahjongBuddy.Application.Errors;
using MahjongBuddy.Core;
using MahjongBuddy.EntityFramework.EntityFramework;
using MediatR;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace MahjongBuddy.Application.Games
{
    public class Detail
    {
        public class Query : IRequest<GameDto>
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, GameDto>
        {
            private readonly MahjongBuddyDbContext _context;
            private readonly IMapper _mapper;

            public Handler(MahjongBuddyDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<GameDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var game = await _context.Games
                    .FindAsync(request.Id);

                if (game == null)
                    throw new RestException(HttpStatusCode.BadRequest, new { game = "Not Found" });

                var gameToReturn = _mapper.Map<Game, GameDto>(game);

                return gameToReturn;
            }
        }
    }
}
