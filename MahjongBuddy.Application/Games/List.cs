using AutoMapper;
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
        public class Query : IRequest<List<GameDto>> { }

        public class Handler : IRequestHandler<Query, List<GameDto>>
        {
            private readonly MahjongBuddyDbContext _context;
            private readonly IMapper _mapper;

            public Handler(MahjongBuddyDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<GameDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var games = await _context.Games
                    .ToListAsync();

                return _mapper.Map<List<Game>, List<GameDto>>(games);
            }
        }
    }
}
