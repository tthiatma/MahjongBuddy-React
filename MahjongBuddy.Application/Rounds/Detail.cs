using AutoMapper;
using MahjongBuddy.Application.Dtos;
using MahjongBuddy.Application.Errors;
using MahjongBuddy.Core;
using MahjongBuddy.EntityFramework.EntityFramework;
using MediatR;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace MahjongBuddy.Application.Rounds
{
    public class Detail
    {
        public class Query : IRequest<RoundDto>
        {
            public int Id { get; set; }
        }
        public class Handler : IRequestHandler<Query, RoundDto>
        {
            private readonly MahjongBuddyDbContext _context;
            private readonly IMapper _mapper;

            public Handler(MahjongBuddyDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<RoundDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var round = await _context.Rounds
                    .FindAsync(request.Id);

                if (round == null)
                    throw new RestException(HttpStatusCode.NotFound, new { round = "Not Found" });

                var roundToReturn = _mapper.Map<Round, RoundDto>(round);

                return roundToReturn;
            }
        }
    }
}
