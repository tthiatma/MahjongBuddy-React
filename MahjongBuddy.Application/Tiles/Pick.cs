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
    public class Pick
    {
        public class Command : IRequest<RoundDto>
        {
            public string GameId { get; set; }
            public int RoundId { get; set; }
            public string UserName { get; set; }
        }
        public class Handler : IRequestHandler<Command, RoundDto>
        {
            private readonly MahjongBuddyDbContext _context;
            private readonly IMapper _mapper;

            public Handler(MahjongBuddyDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<RoundDto> Handle(Command request, CancellationToken cancellationToken)
            {
                //Note to consider when picking tile:
                //-tile is flower
                //-no more tile to pick
                //-only 1 more tile to pick because player have an option not to take the last tile.

                var updatedTiles = new List<RoundTile>();
                var round = await _context.Rounds.FindAsync(request.RoundId);

                if (round == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Round = "Could not find round" });

                //we loop 8 times because there are total of 8 flowers. get more tiles if its a flower
                for (var i = 0; i < 8; i++)
                {
                    var newTile = round.RoundTiles.FirstOrDefault(t => string.IsNullOrEmpty(t.Owner));
                    if (newTile == null)
                    {
                        round.IsOver = true;
                        break;
                    }

                    newTile.Owner = request.UserName;
                    round.IsHalted = true;

                    if (newTile.Tile.TileType != TileType.Flower)
                    {
                        newTile.Status = TileStatus.UserJustPicked;
                        updatedTiles.Add(newTile);
                        break;
                    }
                    else
                    {
                        newTile.Status = TileStatus.UserGraveyard;
                        updatedTiles.Add(newTile);
                    }
                }

                var success = await _context.SaveChangesAsync() > 0;

                var roundToReturn = _mapper.Map<Round, RoundDto>(round);

                roundToReturn.UpdatedRoundTiles = _mapper.Map<ICollection<RoundTile>, ICollection<RoundTileDto>>(updatedTiles);

                if (success)
                    return roundToReturn;

                throw new Exception("Problem picking tile");
            }
        }
    }
}
