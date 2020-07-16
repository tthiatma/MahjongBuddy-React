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
        public class Command : IRequest<RoundDto>
        {
            public int GameId { get; set; }
            public int RoundId { get; set; }
            public Guid TileId { get; set; }
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
                //TODO: when someone can pong/chow the tile, turn on the flag so client can show user the option
                var updatedTiles = new List<RoundTile>();
                var updatedPlayers = new List<RoundPlayer>();
                var round = await _context.Rounds.FindAsync(request.RoundId);

                if (round == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Round = "Could not find round" });

                var currentPlayer = round.RoundPlayers.FirstOrDefault(p => p.AppUser.UserName == request.UserName);

                if(currentPlayer == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Round = "Could not find current player" });

                //existing active tile on board to be no longer active
                var existingActiveTileOnBoard = round.RoundTiles.FirstOrDefault(t => t.Status == TileStatus.BoardActive);
                if (existingActiveTileOnBoard != null)
                {
                    existingActiveTileOnBoard.Status = TileStatus.BoardGraveyard;
                    updatedTiles.Add(existingActiveTileOnBoard);
                }

                var userJustPickedTile = round.RoundTiles.Where(t => t.Owner == request.UserName && t.Status == TileStatus.UserJustPicked);
                if(userJustPickedTile != null && userJustPickedTile.Count() > 0)
                {
                    foreach (var t in userJustPickedTile)
                    {
                        t.Status = TileStatus.UserActive;
                        updatedTiles.Add(t);
                    }
                }

                var tileToThrow = round.RoundTiles.FirstOrDefault(t => t.Id == request.TileId);

                if (tileToThrow == null)
                    throw new RestException(HttpStatusCode.NotFound, new { RoundTile = "Could not find the tile" });

                tileToThrow.ThrownBy = request.UserName;
                tileToThrow.Owner = DefaultValue.board;
                tileToThrow.Status = TileStatus.BoardActive;
                tileToThrow.BoardGraveyardCounter = round.TileCounter;
                round.TileCounter++;

                updatedTiles.Add(tileToThrow);

                //take off turn from current player and figure out next player's turn
                currentPlayer.IsMyTurn = false;
                var nextPlayerTurn = GetNextPlayer(round.RoundPlayers, currentPlayer.Wind);
                nextPlayerTurn.IsMyTurn = true;

                updatedPlayers.Add(currentPlayer);
                updatedPlayers.Add(nextPlayerTurn);

                //check if theres more remaining tile, if no more tiles, then set round to ending
                var remainingTiles = round.RoundTiles.FirstOrDefault(t => string.IsNullOrEmpty(t.Owner));
                if (remainingTiles == null)
                    round.IsEnding = true;

                var success = await _context.SaveChangesAsync() > 0;

                var roundToReturn = _mapper.Map<Round, RoundDto>(round);

                roundToReturn.UpdatedRoundTiles = _mapper.Map<ICollection<RoundTile>, ICollection<RoundTileDto>>(updatedTiles);
                roundToReturn.UpdatedRoundPlayers = _mapper.Map<ICollection<RoundPlayer>, ICollection<RoundPlayerDto>>(updatedPlayers);

                if (success)
                    return roundToReturn;

                throw new Exception("Problem throwing tile");
            }

            private RoundPlayer GetNextPlayer(ICollection<RoundPlayer> players, WindDirection currentPlayerWind)
            {
                RoundPlayer ret;
                switch (currentPlayerWind)
                {
                    case WindDirection.East:
                        ret = players.First(p => p.Wind == WindDirection.North);
                        break;
                    case WindDirection.South:
                        ret = players.First(p => p.Wind == WindDirection.East);
                        break;
                    case WindDirection.West:
                        ret = players.First(p => p.Wind == WindDirection.South);
                        break;
                    case WindDirection.North:
                        ret = players.First(p => p.Wind == WindDirection.West);
                        break;
                    default:
                        throw new Exception("Error when getting next player");
                }

                return ret;
            }
        }
    }
}
