using AutoMapper;
using MahjongBuddy.Application.Dtos;
using MahjongBuddy.Application.Errors;
using MahjongBuddy.Core;
using MahjongBuddy.EntityFramework.EntityFramework;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace MahjongBuddy.Application.Rounds
{
    public class GetRoundTiles
    {
        public class Query: IRequest<List<RoundTileDto>>
        {
            public int Id { get; set; }
            public int GameId { get; set; }
        }

        public class Handler: IRequestHandler<Query, List<RoundTileDto>>  
        {
            private readonly MahjongBuddyDbContext _context;
            private readonly IMapper _mapper;

            public Handler(MahjongBuddyDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<RoundTileDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var game = await _context.Games.FindAsync(request.GameId);
                if (game == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Game = "Could not find game" });

                var round = game.Rounds.FirstOrDefault(r => r.Id == request.Id);
                if (round == null)
                    throw new RestException(HttpStatusCode.NotFound, new { round = "Could Not find round" });

                var roundTiles = round.RoundTiles.ToList();

                if(roundTiles == null)
                    throw new RestException(HttpStatusCode.NotFound, new { game = "No tiles Found for this round" });

                var roundTilestoReturn = _mapper.Map<List<RoundTile>, List<RoundTileDto>>(roundTiles);

                return roundTilestoReturn;
            }
        }
    }
}
