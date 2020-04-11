using AutoMapper;
using MahjongBuddy.Application.Errors;
using MahjongBuddy.Core;
using MahjongBuddy.EntityFramework.EntityFramework;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace MahjongBuddy.Application.Rounds
{
    public class GetRoundTiles
    {
        public class Command: IRequest<List<RoundTileDto>>
        {
            public int Id { get; set; }
        }

        public class Handler: IRequestHandler<Command, List<RoundTileDto>>  
        {
            private readonly MahjongBuddyDbContext _context;
            private readonly IMapper _mapper;

            public Handler(MahjongBuddyDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<RoundTileDto>> Handle(Command request, CancellationToken cancellationToken)
            {
                var roundTiles = await _context.RoundTiles.Where(x => x.RoundId == request.Id).ToListAsync();

                if(roundTiles == null)
                    throw new RestException(HttpStatusCode.NotFound, new { game = "No tiles Found for this round" });

                var roundTilestoReturn = _mapper.Map<List<RoundTile>, List<RoundTileDto>>(roundTiles);

                return roundTilestoReturn;
            }
        }
    }
}
