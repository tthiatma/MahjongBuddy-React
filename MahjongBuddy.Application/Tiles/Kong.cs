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
using MahjongBuddy.Application.Extensions;

namespace MahjongBuddy.Application.Tiles
{
    public class Kong
    {
        public class Command : IRequest<RoundDto>
        {
            public string GameId { get; set; }
            public int RoundId { get; set; }
            public string UserName { get; set; }
            public TileType TileType { get; set; }
            public TileValue TileValue { get; set; }
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
                //Note to consider when kong tile:
                //if player has pong in their graveyard, it can be kong
                //-player turn changed
                //-flag when user can kong
                //-when kong tile user need to grab a new tile.
                //-kong doesnt rely on board active tile when its "User's turn", it could be just from player list of active tiles
                //-user can kong anytime when it's their turn n it could be more than 1 set to kong
                //assign tile ownership to current user
                //weird situation is when it's user's turn, user can kong their active tiles and can kong board active tiles

                var updatedTiles = new List<RoundTile>();
                var updatedPlayers = new List<UserRound>();

                var round = await _context.Rounds.FindAsync(request.RoundId);

                if (round == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Round = "Could not find round" });

                round.IsHalted = true;

                var currentPlayer = round.UserRounds.FirstOrDefault(u => u.AppUser.UserName == request.UserName);
                if (currentPlayer == null)
                    throw new RestException(HttpStatusCode.BadRequest, new { Round = "there are no user with this username in the round" });

                var boardActiveTiles = round.RoundTiles.Where(t => t.Status == TileStatus.BoardActive);
                if (boardActiveTiles == null || boardActiveTiles.Count() > 1)
                    throw new RestException(HttpStatusCode.BadRequest, new { Round = "there are more than one tiles to kong" });

                var bat = boardActiveTiles.First();
                var boardActiveMatchedWithRequest = (bat.Tile.TileType == request.TileType && bat.Tile.TileValue == request.TileValue);

                List<RoundTile> userActiveTilesToKong = new List<RoundTile>();
                var currentUserTiles = round.RoundTiles.Where(t => t.Owner == request.UserName);

                //if it's user's turn, check if user has all 4 same tiles first in the user active tiles
                //because the request has tile type and tile value, there can only one possible kong
                //if user has pong tile, user can kong matching tile
                var tilesToKong = currentUserTiles
                .Where(t => t.Tile.TileType == request.TileType 
                && t.Tile.TileValue == request.TileValue);

                if (currentPlayer.IsMyTurn)
                {
                    if(tilesToKong.Count() == 4)
                    {
                        updatedTiles.AddRange(tilesToKong);
                    }
                    if (tilesToKong.Count() == 3)
                    {                    
                        var userRequestedTileTokong = tilesToKong.First(t => t.Status != TileStatus.UserGraveyard );

                        if(userRequestedTileTokong != null)
                        {
                            updatedTiles.AddRange(tilesToKong);
                            updatedTiles.Add(userRequestedTileTokong);
                        }
                        if(boardActiveMatchedWithRequest)
                        {
                            updatedTiles.AddRange(tilesToKong);
                            updatedTiles.Add(bat);
                        }
                    }
                }
                else
                {
                    //not user's turn, so can only kong from board active tile
                    if (tilesToKong.Count() == 3 && boardActiveMatchedWithRequest)
                    {
                        updatedTiles.AddRange(tilesToKong);
                        updatedTiles.Add(bat);
                    }
                }

                if(updatedTiles.Count() == 4)
                    updatedTiles.GoGraveyard(request.UserName, TileSetGroup.Kong);
                else
                    throw new RestException(HttpStatusCode.BadRequest, new { Kong = "Not possible to kong" });
                //add new tile for user
                var newTiles = round.RoundTiles.PickTile(request.UserName);

                if(newTiles == null)
                //TODO: check when there is no more tile then user kong and game is over?

                //assign new tile to user that kong the tile
                foreach (var tile in newTiles)
                {
                    updatedTiles.Add(tile);
                }

                currentPlayer.IsMyTurn = true;

                var otherPlayerTurn = round.UserRounds.FirstOrDefault(u => u.IsMyTurn == true);
                if(otherPlayerTurn != null)
                {
                    otherPlayerTurn.IsMyTurn = false;
                    updatedPlayers.Add(otherPlayerTurn);
                }

                updatedPlayers.Add(currentPlayer);

                var success = await _context.SaveChangesAsync() > 0;

                var roundToReturn = _mapper.Map<Round, RoundDto>(round);

                roundToReturn.UpdatedRoundTiles = _mapper.Map<ICollection<RoundTile>, ICollection<RoundTileDto>>(updatedTiles);
                roundToReturn.UpdatedRoundPlayers = _mapper.Map<ICollection<UserRound>, ICollection<RoundPlayerDto>>(updatedPlayers);

                if (success)
                    return roundToReturn;

                throw new Exception("Problem kong ing tile");
            }
        }
    }
}
