using AutoMapper;
using MahjongBuddy.Application.Dtos;
using MahjongBuddy.Application.Errors;
using MahjongBuddy.Application.Extensions;
using MahjongBuddy.Application.Helpers;
using MahjongBuddy.Core;
using MahjongBuddy.EntityFramework.EntityFramework;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MoreLinq;
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
        public class Command : IRequest<IEnumerable<RoundDto>>
        {
            public int RoundId { get; set; }
            public string UserName { get; set; }
            public IEnumerable<Guid> ChowTiles { get; set; }
        }
        public class Handler : IRequestHandler<Command, IEnumerable<RoundDto>>
        {
            private readonly MahjongBuddyDbContext _context;
            private readonly IMapper _mapper;

            public Handler(MahjongBuddyDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<IEnumerable<RoundDto>> Handle(Command request, CancellationToken cancellationToken)
            {
                //Note to consider when chow tile:
                //-when chow tile user need to throw a tile.
                //assign tile ownership to current user
                var round = await _context.Rounds.FindAsync(request.RoundId);

                if (round == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Round = "Could not find round" });

                //TODO only allow chow when the user that requested chow is after the player that has turn

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

                var currentPlayer = round.RoundPlayers.FirstOrDefault(u => u.GamePlayer.Player.UserName == request.UserName);
                if (currentPlayer == null)
                    throw new RestException(HttpStatusCode.BadRequest, new { Round = "there are no user with this username in the round" });

                currentPlayer.IsMyTurn = true;
                currentPlayer.MustThrow = true;

                var otherPlayers = round.RoundPlayers.Where(u => u.IsMyTurn == true && u.GamePlayer.Player.UserName != request.UserName);
                foreach (var otherPlayerTurn in otherPlayers)
                {
                    otherPlayerTurn.IsMyTurn = false;
                    otherPlayerTurn.MustThrow = false;
                }

                if (round.IsEnding)
                    round.IsEnding = false;

                //if user chow, then there's no way that user can win/pong/chow
                var actionsToBeRemoved = currentPlayer.RoundPlayerActions.Where(a => a.ActionType != ActionType.SelfKong).ToList();
                foreach (var action in actionsToBeRemoved)
                {
                    currentPlayer.RoundPlayerActions.Remove(action);
                }
                RoundHelper.CheckPossibleSelfKong(round, currentPlayer);

                try
                {
                    var success = await _context.SaveChangesAsync() > 0;

                    List<RoundDto> results = new List<RoundDto>();

                    foreach (var p in round.RoundPlayers)
                    {
                        results.Add(_mapper.Map<Round, RoundDto>(round, opt => opt.Items["MainRoundPlayer"] = p));
                    }

                    if (success) return results;
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
