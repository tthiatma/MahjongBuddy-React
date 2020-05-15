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
    public class Chow
    {
        public class Command : IRequest<RoundDto>
        {
            public string GameId { get; set; }
            public int RoundId { get; set; }
            public string UserName { get; set; }
            public IEnumerable<Guid> ChowTiles { get; set; }
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
                //Note to consider when chow tile:
                //-when chow tile user need to throw a tile.
                //assign tile ownership to current user

                var updatedTiles = new List<RoundTile>();

                var round = await _context.Rounds.FindAsync(request.RoundId);

                if (round == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Round = "Could not find round" });

                round.IsHalted = true;

                if(request.ChowTiles == null || request.ChowTiles.Count() != 2)
                    throw new RestException(HttpStatusCode.BadRequest, new { Round = "invalid data to chow tiles" });

                var boardActiveTiles = round.RoundTiles.Where(t => t.Status == TileStatus.BoardActive);
                if (boardActiveTiles == null || boardActiveTiles.Count() > 1)
                    throw new RestException(HttpStatusCode.BadRequest, new { Round = "there are no tile or more than one tiles to chow" });

                var tileToChow = boardActiveTiles.First();

                if(tileToChow.Tile.TileType == TileType.Dragon || tileToChow.Tile.TileType == TileType.Flower || tileToChow.Tile.TileType == TileType.Wind)
                    throw new RestException(HttpStatusCode.BadRequest, new { Round = "tile must be money, round, or stick for chow" });

                var dbTilesToChow = round.RoundTiles.Where(t => request.ChowTiles.Contains(t.Id)).ToList();

                dbTilesToChow.Add(tileToChow);

                var sortedChowTiles = dbTilesToChow.OrderBy(t => t.Tile.TileValue).ToArray();

                //check if its straight
                if (sortedChowTiles[0].Tile.TileValue + 1 == sortedChowTiles[1].Tile.TileValue
                    && sortedChowTiles[1].Tile.TileValue + 1 == sortedChowTiles[2].Tile.TileValue)
                {
                    foreach (var tile in sortedChowTiles)
                    {
                        tile.Owner = request.UserName;
                        tile.TileSetGroup = TileSetGroup.Chow;
                        tile.Status = TileStatus.UserGraveyard;
                    }
                }
                else
                    throw new RestException(HttpStatusCode.BadRequest, new { Round = "tile is not in sequence to chow" });

                updatedTiles.AddRange(sortedChowTiles);

                var currentPlayer = round.RoundPlayers.FirstOrDefault(u => u.AppUser.UserName == request.UserName);
                if (currentPlayer == null)
                    throw new RestException(HttpStatusCode.BadRequest, new { Round = "there are no user with this username in the round" });

                var success = await _context.SaveChangesAsync() > 0;

                var roundToReturn = _mapper.Map<Round, RoundDto>(round);

                roundToReturn.UpdatedRoundTiles = _mapper.Map<ICollection<RoundTile>, ICollection<RoundTileDto>>(updatedTiles);

                if (success)
                    return roundToReturn;

                throw new Exception("Problem chow ing tile");
            }
        }
    }
}
