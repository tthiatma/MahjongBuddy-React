using AutoMapper;
using MahjongBuddy.Application.Dtos;
using MahjongBuddy.Application.Errors;
using MahjongBuddy.Application.Extensions;
using MahjongBuddy.Core;
using MahjongBuddy.EntityFramework.EntityFramework;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace MahjongBuddy.Application.PlayerAction
{
    public class Chow
    {
        public class Command : IRequest<RoundDto>
        {
            public int GameId { get; set; }
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
                var updatedPlayers = new List<RoundPlayer>();

                var round = await _context.Rounds.FindAsync(request.RoundId);

                if (round == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Round = "Could not find round" });

                round.IsHalted = true;

                //TODO only allow chow when it's user's turn

                if (request.ChowTiles == null || request.ChowTiles.Count() != 2)
                    throw new RestException(HttpStatusCode.BadRequest, new { Round = "invalid data to chow tiles" });

                var boardActiveTiles = round.RoundTiles.Where(t => t.Status == TileStatus.BoardActive);
                if (boardActiveTiles == null || boardActiveTiles.Count() > 1)
                    throw new RestException(HttpStatusCode.BadRequest, new { Round = "there are no tile or more than one tiles to chow" });

                var tileToChow = boardActiveTiles.First();

                if (tileToChow.Tile.TileType == TileType.Dragon || tileToChow.Tile.TileType == TileType.Flower || tileToChow.Tile.TileType == TileType.Wind)
                    throw new RestException(HttpStatusCode.BadRequest, new { Round = "tile must be money, round, or stick for chow" });

                var dbTilesToChow = round.RoundTiles.Where(t => request.ChowTiles.Contains(t.Id)).ToList();

                dbTilesToChow.Add(tileToChow);

                var sortedChowTiles = dbTilesToChow.OrderBy(t => t.Tile.TileValue).ToArray();

                //check if its straight
                if (sortedChowTiles[0].Tile.TileValue + 1 == sortedChowTiles[1].Tile.TileValue
                    && sortedChowTiles[1].Tile.TileValue + 1 == sortedChowTiles[2].Tile.TileValue)
                {

                    int groupIndex = 1;
                    var tileSets = round.RoundTiles.Where(t => t.Owner == request.UserName && t.TileSetGroup != TileSetGroup.None);
                    if (tileSets.Count() > 0)
                    {
                        var lastIndex = tileSets.GroupBy(t => t.TileSetGroupIndex).Select(g => g.Last()).First().TileSetGroupIndex;
                        groupIndex = lastIndex + 1;
                    }

                    sortedChowTiles.GoGraveyard(request.UserName, TileSetGroup.Chow, round.RoundTiles.GetLastGroupIndex(request.UserName));
                }
                else
                    throw new RestException(HttpStatusCode.BadRequest, new { Round = "tile is not in sequence to chow" });

                updatedTiles.AddRange(sortedChowTiles);

                var currentPlayer = round.RoundPlayers.FirstOrDefault(u => u.AppUser.UserName == request.UserName);
                if (currentPlayer == null)
                    throw new RestException(HttpStatusCode.BadRequest, new { Round = "there are no user with this username in the round" });
                updatedPlayers.Add(currentPlayer);

                currentPlayer.IsMyTurn = true;
                currentPlayer.MustThrow = true;

                var otherPlayers = round.RoundPlayers.Where(u => u.IsMyTurn == true && u.AppUser.UserName != request.UserName);
                foreach (var otherPlayerTurn in otherPlayers)
                {
                    otherPlayerTurn.IsMyTurn = false;
                    otherPlayerTurn.MustThrow = false;
                    updatedPlayers.Add(otherPlayerTurn);
                }

                if (round.IsEnding)
                    round.IsEnding = false;

                try
                {
                    var success = await _context.SaveChangesAsync() > 0;
                    var roundToReturn = _mapper.Map<Round, RoundDto>(round);

                    roundToReturn.UpdatedRoundTiles = _mapper.Map<ICollection<RoundTile>, ICollection<RoundTileDto>>(updatedTiles);
                    roundToReturn.UpdatedRoundPlayers = _mapper.Map<ICollection<RoundPlayer>, ICollection<RoundPlayerDto>>(updatedPlayers);

                    if (success) return roundToReturn;
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw new RestException(HttpStatusCode.BadRequest, new { Round = "tile was modified" });
                }

                throw new Exception("Problem chow ing tile");
            }
        }
    }
}
