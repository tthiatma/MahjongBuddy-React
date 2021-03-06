﻿using AutoMapper;
using MahjongBuddy.Application.Dtos;
using MahjongBuddy.Application.Errors;
using MahjongBuddy.Application.Helpers;
using MahjongBuddy.Application.Interfaces;
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
        public class Command : IRequest<IEnumerable<RoundDto>>
        {
            public int RoundId { get; set; }
            public Guid TileId { get; set; }
            public string UserName { get; set; }
        }
        public class Handler : IRequestHandler<Command, IEnumerable<RoundDto>>
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

            public async Task<IEnumerable<RoundDto>> Handle(Command request, CancellationToken cancellationToken)
            {
                var round = await _context.Rounds.FindAsync(request.RoundId);
                if (round == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Round = "Could not find round" });

                var tileToThrow = round.RoundTiles.FirstOrDefault(t => t.Id == request.TileId);
                if (tileToThrow == null)
                    throw new RestException(HttpStatusCode.NotFound, new { RoundTile = "Could not find the tile" });

                var currentPlayer = round.RoundPlayers.FirstOrDefault(p => p.GamePlayer.Player.UserName == request.UserName);
                if (currentPlayer == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Round = "Could not find current player" });

                if (!currentPlayer.MustThrow && !currentPlayer.IsMyTurn)
                    throw new RestException(HttpStatusCode.NotFound, new { Round = "Player not suppose to throw" });

                //clear all player actions initially every time throw command invoked
                round.RoundPlayers.ForEach(p =>
                {
                    p.RoundPlayerActions.Clear();
                });

                //previous active tile on board to be no longer active
                var existingActiveTileOnBoard = round.RoundTiles.FirstOrDefault(t => t.Status == TileStatus.BoardActive);
                if (existingActiveTileOnBoard != null)
                {
                    existingActiveTileOnBoard.Status = TileStatus.BoardGraveyard;
                }

                //mark current user's just picked tile to be active
                var userJustPickedTile = round.RoundTiles.Where(t => t.Owner == request.UserName && t.Status == TileStatus.UserJustPicked);
                if (userJustPickedTile != null && userJustPickedTile.Count() > 0)
                {
                    userJustPickedTile.ForEach(t =>
                    {
                        t.Status = TileStatus.UserActive;
                    });
                }

                //update thrown tile props and increase the tilecounter
                tileToThrow.ThrownBy = request.UserName;
                tileToThrow.Owner = DefaultValue.board;
                tileToThrow.Status = TileStatus.BoardActive;
                tileToThrow.BoardGraveyardCounter = round.TileCounter;
                round.TileCounter++;

                //don't change user's turn if there is player with action 
                //----------------------------------------------------------
                var gotAction = AssignPlayerActions(round, currentPlayer);

                if (gotAction)
                {
                    //action has priority list: win > pong|kong > chow
                    var winActionPlayer = round.RoundPlayers.Where(rp => rp.RoundPlayerActions.Any(rpa => rpa.ActionType == ActionType.Win));
                    if (winActionPlayer.Count() > 0)
                    {
                        bool multipleWinners = winActionPlayer.Count() > 1;
                        foreach (var winner in winActionPlayer)
                        {
                            if (multipleWinners)
                                winner.RoundPlayerActions.Where(ac => ac.ActionType == ActionType.Win).ForEach(a => a.ActionStatus = ActionStatus.Active);
                            else
                                winner.RoundPlayerActions.ForEach(a => a.ActionStatus = ActionStatus.Active);
                        }
                    }
                    else
                    {
                        var pongOrKongActionPlayer = round.RoundPlayers.Where(
                            rp => rp.RoundPlayerActions.Any(
                                rpa => rpa.ActionType == ActionType.Pong ||
                                rpa.ActionType == ActionType.Kong
                                )
                            ).FirstOrDefault();
                        if (pongOrKongActionPlayer != null)
                        {
                            pongOrKongActionPlayer.RoundPlayerActions.ForEach(a => a.ActionStatus = ActionStatus.Active);
                        }
                        //check if next player has chow action 
                        else
                        {
                            var chowActionPlayer = round.RoundPlayers.Where(rp => rp.RoundPlayerActions.Any(rpa => rpa.ActionType == ActionType.Chow)).FirstOrDefault();
                            if (chowActionPlayer != null)
                            {
                                chowActionPlayer.RoundPlayerActions.ForEach(a => a.ActionStatus = ActionStatus.Active);
                            }
                        }
                    }
                }
                else
                {
                    RoundHelper.SetNextPlayer(round, _pointCalculator);

                    currentPlayer.IsMyTurn = false;

                    //check if theres more remaining tile, if no more tiles, then set round to ending
                    var remainingTiles = round.RoundTiles.FirstOrDefault(t => string.IsNullOrEmpty(t.Owner));
                    if (remainingTiles == null)
                        round.IsEnding = true;
                }
                currentPlayer.MustThrow = false;

                if (!currentPlayer.IsManualSort)
                {
                    var playerAliveTiles = round.RoundTiles.Where(rt => rt.Owner == request.UserName && (rt.Status == TileStatus.UserActive || rt.Status == TileStatus.UserJustPicked)).ToList();
                    RoundTileHelper.AssignAliveTileCounter(playerAliveTiles);
                }

                var success = await _context.SaveChangesAsync() > 0;

                List<RoundDto> results = new List<RoundDto>();

                foreach (var p in round.RoundPlayers)
                {
                    results.Add(_mapper.Map<Round, RoundDto>(round, opt => opt.Items["MainRoundPlayer"] = p));
                }

                if (success)
                    return results;

                throw new Exception("Problem throwing tile");
            }

            private bool AssignPlayerActions(Round round, RoundPlayer throwerPlayer)
            {
                //TODO: Support multiple winner 
                bool foundActionForUser = false;
                var roundTiles = round.RoundTiles;

                //there will be action except for the player that throw the tile 
                var players = round.RoundPlayers.Where(rp => rp.GamePlayer.Player.UserName != throwerPlayer.GamePlayer.Player.UserName);

                var boardActiveTile = roundTiles.FirstOrDefault(rt => rt.Status == TileStatus.BoardActive);
                if (boardActiveTile == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Round = "Could not find active board tile" });

                var nextPlayer = RoundHelper.GetNextPlayer(round.RoundPlayers, throwerPlayer.Wind);

                //there could be more than one possible action given user's turn  and the tile's thrown
                foreach (var player in players)
                {
                    var userTiles = roundTiles.Where(rt => rt.Owner == player.GamePlayer.Player.UserName);
                    List<RoundPlayerAction> rpas = new List<RoundPlayerAction>();

                    if (RoundHelper.DetermineIfUserCanWin(round, player, _pointCalculator))
                        rpas.Add(new RoundPlayerAction { ActionType = ActionType.Win, ActionStatus = ActionStatus.Inactive });

                    if (DetermineIfUserCanKongFromBoard(userTiles, boardActiveTile))
                        rpas.Add(new RoundPlayerAction { ActionType = ActionType.Kong, ActionStatus = ActionStatus.Inactive });

                    if (DetermineIfUserCanPong(userTiles, boardActiveTile))
                        rpas.Add(new RoundPlayerAction { ActionType = ActionType.Pong, ActionStatus = ActionStatus.Inactive });

                    if (player.GamePlayer.Player.UserName == nextPlayer.GamePlayer.Player.UserName)
                    {
                        var nextPlayerTiles = round.RoundTiles.Where(rt => rt.Owner == nextPlayer.GamePlayer.Player.UserName);
                        if (DetermineIfUserCanChow(nextPlayerTiles, boardActiveTile))
                            rpas.Add(new RoundPlayerAction { ActionType = ActionType.Chow, ActionStatus = ActionStatus.Inactive });
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
                if (boardTile.Tile.TileType == TileType.Wind || boardTile.Tile.TileType == TileType.Dragon || boardTile.Tile.TileType == TileType.Flower)
                    return false;

                var possibleChowTiles = RoundTileHelper.FindPossibleChowTiles(boardTile, playerTiles);

                return possibleChowTiles.Count() >= 1;
            }
        }
    }
}
