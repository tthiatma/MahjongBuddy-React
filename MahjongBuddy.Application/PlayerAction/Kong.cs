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
using Microsoft.EntityFrameworkCore;
using MahjongBuddy.Application.Helpers;
using Microsoft.EntityFrameworkCore.Internal;
using MahjongBuddy.Application.Interfaces;
using MoreLinq;

namespace MahjongBuddy.Application.PlayerAction
{
    public class Kong
    {
        public class Command : IRequest<RoundDto>
        {
            public int GameId { get; set; }
            public int RoundId { get; set; }
            public string UserName { get; set; }
            public TileType TileType { get; set; }
            public TileValue TileValue { get; set; }
        }
        public class Handler : IRequestHandler<Command, RoundDto>
        {
            private readonly MahjongBuddyDbContext _context;
            private readonly IMapper _mapper;
            private readonly IPointsCalculator _pointsCalculator;

            public Handler(MahjongBuddyDbContext context, IMapper mapper, IPointsCalculator pointsCalculator)
            {
                _context = context;
                _mapper = mapper;
                _pointsCalculator = pointsCalculator;
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
                var updatedPlayers = new List<RoundPlayer>();

                var round = await _context.Rounds.FindAsync(request.RoundId);

                if (round == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Round = "Could not find round" });

                round.IsHalted = true;

                var currentPlayer = round.RoundPlayers.FirstOrDefault(u => u.AppUser.UserName == request.UserName);
                if (currentPlayer == null)
                    throw new RestException(HttpStatusCode.BadRequest, new { Round = "there are no user with this username in the round" });

                List<RoundTile> userActiveTilesToKong = new List<RoundTile>();
                var currentUserTiles = round.RoundTiles.Where(t => t.Owner == request.UserName);

                //if it's user's turn, check if user has all 4 same tiles first in the user active tiles
                //because the request has tile type and tile value, there can only one possible kong
                //if user has pong tile, user can kong matching tile
                var tilesToKong = currentUserTiles
                .Where(t => t.Tile.TileType == request.TileType 
                && t.Tile.TileValue == request.TileValue);

                if(tilesToKong.Count() == 4)
                {
                    if(currentPlayer.IsMyTurn)
                    {
                        //if its not konged already, then user can kong it
                        var kongedTile = tilesToKong.Where(t => t.TileSetGroup == TileSetGroup.Kong);
                        if (kongedTile.Count() == 0)
                        {
                            updatedTiles.AddRange(tilesToKong);
                        }
                    }
                }

                //this means that user need to kong from board
                if (tilesToKong.Count() == 3)
                {
                    //if user only have three and its already ponged, then player can't kong
                    var tilesAlreadyPonged = tilesToKong.Where(t => t.TileSetGroup == TileSetGroup.Pong);
                    if(tilesAlreadyPonged.Count() == 3)
                        throw new RestException(HttpStatusCode.BadRequest, new { Round = "Can't do kong when all tiles ponged" });

                    var boardActiveTiles = round.RoundTiles.FirstOrDefault(t => t.Status == TileStatus.BoardActive);
                    if (boardActiveTiles == null)
                        throw new RestException(HttpStatusCode.BadRequest, new { Round = "there are no board active tile to kong" });
                    var boardActiveMatchedWithRequest = (boardActiveTiles.Tile.TileType == request.TileType && boardActiveTiles.Tile.TileValue == request.TileValue);

                    if (boardActiveMatchedWithRequest)
                    {
                        //only have 3 active tiles then board must exist to kong
                        var allTilesAreActive = tilesToKong.Where(t => t.Status != TileStatus.UserGraveyard);
                        if (boardActiveMatchedWithRequest && allTilesAreActive.Count() == 3)
                        {
                            updatedTiles.AddRange(tilesToKong);
                            updatedTiles.Add(boardActiveTiles);
                        }
                    }
                }                

                if(updatedTiles.Count() == 4)
                    updatedTiles.GoGraveyard(request.UserName, TileSetGroup.Kong, round.RoundTiles.GetLastGroupIndex(request.UserName));
                else
                    throw new RestException(HttpStatusCode.BadRequest, new { Kong = "Not possible to kong" });

                //set existing justpicked tile to useractive;
                var existingJustPicked = round.RoundTiles.FirstOrDefault(rt => rt.Owner == request.UserName && rt.Status == TileStatus.UserJustPicked);
                if (existingJustPicked != null)
                {
                    existingJustPicked.Status = TileStatus.UserActive;
                    updatedTiles.Add(existingJustPicked);
                }

                //clear existing action
                currentPlayer.RoundPlayerActions.Clear();

                //add new tile for user                
                var newTiles = RoundTileHelper.PickTile(round, request.UserName, ref updatedTiles, true);

                if(newTiles.Count() > 0)
                {
                    //assign new tile to user that kong the tile
                    foreach (var tile in newTiles)
                    {
                        updatedTiles.Add(tile);
                    }
                    RoundHelper.CheckSelfAction(round, currentPlayer, _pointsCalculator);
                }
                else
                {
                    //TODO: what if user kong when there is no more tile
                }

                currentPlayer.IsMyTurn = true;
                //because new tile automatically added, player must throw set to true
                currentPlayer.MustThrow = true;

                if (round.IsEnding)
                    round.IsEnding = false;

                var otherPlayers = round.RoundPlayers.Where(u => u.IsMyTurn == true && u.AppUser.UserName != request.UserName);
                foreach (var otherPlayerTurn in otherPlayers)
                {
                    otherPlayerTurn.IsMyTurn = false;
                    updatedPlayers.Add(otherPlayerTurn);
                }

                updatedPlayers.Add(currentPlayer);

                try
                {
                    var success = await _context.SaveChangesAsync() > 0;

                    var roundToReturn = _mapper.Map<Round, RoundDto>(round);

                    roundToReturn.UpdatedRoundTiles = _mapper.Map<ICollection<RoundTile>, ICollection<RoundTileDto>>(updatedTiles);
                    roundToReturn.UpdatedRoundPlayers = _mapper.Map<ICollection<RoundPlayer>, ICollection<RoundPlayerDto>>(updatedPlayers);

                    if (success)
                        return roundToReturn;
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw new RestException(HttpStatusCode.BadRequest, new { Round = "tile was modified" });
                }

                throw new Exception("Problem kong ing tile");
            }
        }
    }
}
