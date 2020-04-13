using AutoMapper;
using MahjongBuddy.Application.Dtos;
using MahjongBuddy.Application.Errors;
using MahjongBuddy.Core;
using MahjongBuddy.EntityFramework.EntityFramework;
using MediatR;
using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
namespace MahjongBuddy.Application.Tiles
{
    public class Throw
    {
        public class Command : IRequest<RoundTileDto>
        {
            public string GameId { get; set; }
            public int RoundId { get; set; }
            public Guid TileId { get; set; }
        }
        public class Handler : IRequestHandler<Command, RoundTileDto>
        {
            private readonly MahjongBuddyDbContext _context;
            private readonly IMapper _mapper;

            public Handler(MahjongBuddyDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<RoundTileDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var round = await _context.Rounds.FindAsync(request.RoundId);

                if (round == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Round = "Could not find round" });

                var tileToThrow = round.RoundTiles.FirstOrDefault(t => t.Id == request.TileId);

                if (tileToThrow == null)
                    throw new RestException(HttpStatusCode.NotFound, new { RoundTile = "Could not find the tile" });

                tileToThrow.Owner = "board";
                tileToThrow.Status = TileStatus.BoardGraveyard;
                tileToThrow.BoardGraveyardCounter = round.TileCounter;
                round.TileCounter++;
                round.RoundCounter++;

                var success = await _context.SaveChangesAsync() > 0;

                var tileToReturn = _mapper.Map<RoundTile, RoundTileDto>(tileToThrow);

                if (success) return tileToReturn;

                throw new Exception("Problem throwing tile");
            }
        }
    }
}
