using AutoMapper;
using MahjongBuddy.Application.Dtos;
using MahjongBuddy.Application.Errors;
using MahjongBuddy.Application.Helpers;
using MahjongBuddy.Application.Interfaces;
using MahjongBuddy.Application.Rounds.Scorings;
using MahjongBuddy.Core;
using MahjongBuddy.EntityFramework.EntityFramework;
using MediatR;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace MahjongBuddy.Application.PlayerAction
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
            private readonly IPointsCalculator _pointCalculator;

            public Handler(MahjongBuddyDbContext context, IMapper mapper, IPointsCalculator pointCalculator)
            {
                _context = context;
                _mapper = mapper;
                _pointCalculator = pointCalculator;
            }

            public async Task<RoundDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var updatedTiles = new List<RoundTile>();
                var updatedPlayers = new List<RoundPlayer>();

                var game = await _context.Games.FindAsync(request.GameId);

                if (game == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Game = "Could not find game" });

                var round = await _context.Rounds.FindAsync(request.RoundId);

                if (round == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Round = "Could not find round" });

                var currentPlayer = round.RoundPlayers.FirstOrDefault(p => p.AppUser.UserName == request.UserName);

                if(currentPlayer == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Round = "Could not find current player" });

                //clear all player actions initially every time throw command invoked
                round.RoundPlayers.ForEach(p => {
                    p.RoundPlayerActions.Clear();
                    p.HasAction = false;
                });
                
                //existing active tile on board to be no longer active
                var existingActiveTileOnBoard = round.RoundTiles.FirstOrDefault(t => t.Status == TileStatus.BoardActive);
                if (existingActiveTileOnBoard != null)
                {
                    existingActiveTileOnBoard.Status = TileStatus.BoardGraveyard;
                    updatedTiles.Add(existingActiveTileOnBoard);
                }

                var userJustPickedTile = round.RoundTiles.Where(t => t.Owner == request.UserName && t.Status == TileStatus.UserJustPicked);
                if (userJustPickedTile != null && userJustPickedTile.Count() > 0)
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

                //don't change user's turn if there is player with action 
                //----------------------------------------------------------
                var gotAction = AssignPlayerActions(round, game, currentPlayer);

                if(gotAction)
                {
                    //action has priority list: win > pong|kong > chow
                    var winActionPlayer = round.RoundPlayers.Where(rp => rp.RoundPlayerActions.Any(rpa => rpa.PlayerAction == ActionType.Win)).FirstOrDefault();
                    if (winActionPlayer != null)
                    {
                        winActionPlayer.HasAction = true;
                        updatedPlayers.Add(winActionPlayer);
                    }
                    else
                    {
                        var pongOrKongActionPlayer = round.RoundPlayers.Where(
                            rp => rp.RoundPlayerActions.Any(
                                rpa => rpa.PlayerAction == ActionType.Pong || 
                                rpa.PlayerAction == ActionType.Kong
                                )
                            ).FirstOrDefault();
                        if (pongOrKongActionPlayer != null )
                        {
                            pongOrKongActionPlayer.HasAction = true;
                            updatedPlayers.Add(pongOrKongActionPlayer);
                        }
                        else
                        {
                            //check if next player has chow action 
                            var chowActionPlayer = round.RoundPlayers.Where(rp => rp.RoundPlayerActions.Any(rpa => rpa.PlayerAction == ActionType.Chow)).FirstOrDefault();
                            if (chowActionPlayer != null)
                            {
                                chowActionPlayer.HasAction = true;
                                updatedPlayers.Add(chowActionPlayer);
                            }
                        }
                    }
                }
                else
                {
                    RoundHelper.SetNextPlayer(round, ref updatedPlayers, ref updatedTiles, _pointCalculator);

                    currentPlayer.IsMyTurn = false;

                    //check if theres more remaining tile, if no more tiles, then set round to ending
                    var remainingTiles = round.RoundTiles.FirstOrDefault(t => string.IsNullOrEmpty(t.Owner));
                    if (remainingTiles == null)
                        round.IsEnding = true;
                }
                currentPlayer.MustThrow = false;
                updatedPlayers.Add(currentPlayer);

                var success = await _context.SaveChangesAsync() > 0;

                var roundToReturn = _mapper.Map<Round, RoundDto>(round);

                roundToReturn.UpdatedRoundTiles = _mapper.Map<ICollection<RoundTile>, ICollection<RoundTileDto>>(updatedTiles);
                roundToReturn.UpdatedRoundPlayers = _mapper.Map<ICollection<RoundPlayer>, ICollection<RoundPlayerDto>>(updatedPlayers);

                if (success)
                    return roundToReturn;

                throw new Exception("Problem throwing tile");
            }

            
            private bool AssignPlayerActions(Round round, Game game, RoundPlayer throwerPlayer)
            {
                //TODO: Support multiple winner 
                bool foundActionForUser = false;
                var roundTiles = round.RoundTiles;
                
                //there will be action except for the player that throw the tile 
                var players = round.RoundPlayers.Where(rp => rp.AppUser.UserName != throwerPlayer.AppUser.UserName);

                var boardActiveTile = roundTiles.FirstOrDefault(rt => rt.Status == TileStatus.BoardActive);
                if(boardActiveTile == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Round = "Could not find active board tile" });

                var nextPlayer = RoundHelper.GetNextPlayer(round.RoundPlayers, throwerPlayer.Wind);

                //there could be more than one possible action given user's turn  and the tile's thrown
                foreach (var player in players)
                {
                    var userTiles = roundTiles.Where(rt => rt.Owner == player.AppUser.UserName);
                    List<RoundPlayerAction> rpas = new List<RoundPlayerAction>();

                    if (RoundHelper.DetermineIfUserCanWin(round, player, _pointCalculator))
                        rpas.Add(new RoundPlayerAction { PlayerAction = ActionType.Win});

                    if(DetermineIfUserCanKongFromBoard(userTiles, boardActiveTile))
                        rpas.Add(new RoundPlayerAction { PlayerAction = ActionType.Kong});

                    if (DetermineIfUserCanPong(userTiles, boardActiveTile))
                        rpas.Add(new RoundPlayerAction { PlayerAction = ActionType.Pong });

                    if(player.AppUser.UserName == nextPlayer.AppUser.UserName)
                    {
                        var nextPlayerTiles = round.RoundTiles.Where(rt => rt.Owner == nextPlayer.AppUser.UserName);
                        if(DetermineIfUserCanChow(nextPlayerTiles, boardActiveTile))
                            rpas.Add(new RoundPlayerAction { PlayerAction = ActionType.Chow });
                    }

                    if (rpas.Count() > 0)
                    {
                        foundActionForUser = true;
                        foreach (var pa in rpas)
                        {
                            player.RoundPlayerActions.Add(pa);
                        }
                    }
                }

                return foundActionForUser;
            }

            private bool DetermineIfUserCanKongFromBoard(IEnumerable<RoundTile> playerTiles, RoundTile boardTile)
            {
                //player tiles can't be in graveyard when kong from board
                //when someone throw a tile, there should not be a justpicked status tile
                playerTiles = playerTiles.Where(rt => rt.Status == TileStatus.UserActive);

                var playerSameTilesAsBoard = playerTiles
                .Where(t => t.Tile.TileType == boardTile.Tile.TileType
                && t.Tile.TileValue == boardTile.Tile.TileValue);

                return playerSameTilesAsBoard.Count() == 3;
            }

            private bool DetermineIfUserCanPong(IEnumerable<RoundTile> playerTiles, RoundTile boardTile)
            {
                //player tiles can't be in graveyard when pong from board
                //when someone throw a tile, there should not be a justpicked status tile
                playerTiles = playerTiles.Where(rt => rt.Status == TileStatus.UserActive);

                var playerSameTilesAsBoard = playerTiles
                .Where(t => t.Tile.TileType == boardTile.Tile.TileType
                && t.Tile.TileValue == boardTile.Tile.TileValue);

                return playerSameTilesAsBoard.Count() >= 2;
            }
            private bool DetermineIfUserCanChow(IEnumerable<RoundTile> playerTiles, RoundTile boardTile)
            {
                //player tiles can't be in graveyard when pong from board
                //when someone throw a tile, there should not be a justpicked status tile
                var possibleChowTiles = RoundTileHelper.FindPossibleChowTiles(boardTile, playerTiles);

                return possibleChowTiles.Count() >= 1;
            }
        }
    }
}
