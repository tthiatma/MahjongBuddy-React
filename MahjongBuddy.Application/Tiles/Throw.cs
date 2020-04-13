using AutoMapper;
using MahjongBuddy.Application.Dtos;
using MahjongBuddy.Application.Errors;
using MahjongBuddy.Core;
using MahjongBuddy.EntityFramework.EntityFramework;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
namespace MahjongBuddy.Application.Tiles
{
    public class Throw
    {
        public class Command : IRequest<List<RoundTileDto>>
        {
            public string GameId { get; set; }
            public int RoundId { get; set; }
            public Guid TileId { get; set; }
        }
        public class Handler : IRequestHandler<Command, List<RoundTileDto>>
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
                var tileList = new List<RoundTile>();
                var round = await _context.Rounds.FindAsync(request.RoundId);

                if (round == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Round = "Could not find round" });

                //existing active tile on board to be no longer active
                var existingActiveTileOnBoard = round.RoundTiles.FirstOrDefault(t => t.Status == TileStatus.BoardActive);
                if (existingActiveTileOnBoard != null)
                {
                    existingActiveTileOnBoard.Status = TileStatus.BoardGraveyard;
                    tileList.Add(existingActiveTileOnBoard);
                }

                var tileToThrow = round.RoundTiles.FirstOrDefault(t => t.Id == request.TileId);

                if (tileToThrow == null)
                    throw new RestException(HttpStatusCode.NotFound, new { RoundTile = "Could not find the tile" });

                tileToThrow.Owner = "board";
                tileToThrow.Status = TileStatus.BoardActive;
                tileToThrow.BoardGraveyardCounter = round.TileCounter;
                round.TileCounter++;
                round.RoundCounter++;

                tileList.Add(tileToThrow);

                var success = await _context.SaveChangesAsync() > 0;

                var tilesToReturn = _mapper.Map<List<RoundTile>, List<RoundTileDto>>(tileList);

                if (success)
                    return tilesToReturn;

                throw new Exception("Problem throwing tile");
            }
        }
    }
}
