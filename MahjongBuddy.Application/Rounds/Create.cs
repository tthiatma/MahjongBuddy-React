﻿using AutoMapper;
using MahjongBuddy.Application.Dtos;
using MahjongBuddy.Application.Errors;
using MahjongBuddy.Application.Extensions;
using MahjongBuddy.Application.Helpers;
using MahjongBuddy.Core;
using MahjongBuddy.Core.Enums;
using MahjongBuddy.EntityFramework.EntityFramework;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace MahjongBuddy.Application.Rounds
{
    public class Create
    {
        public class Command : IRequest<IEnumerable<RoundDto>>
        {
            public string GameCode { get; set; }
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
                var game = await _context.Games.FirstOrDefaultAsync(x => x.Code == request.GameCode.ToUpper());

                if (game == null)
                    throw new RestException(HttpStatusCode.BadRequest, new { Game = "Game does not exist" });

                Round lastRound = game.Rounds.OrderByDescending(r => r.DateCreated).FirstOrDefault();

                if (lastRound != null && !lastRound.IsOver)
                    throw new RestException(HttpStatusCode.BadRequest, new { Round = "Last round is not over" });

                var newRound = new Round
                {
                    GameId = game.Id,
                    DateCreated = DateTime.Now,
                    RoundTiles = RoundTileHelper.CreateTiles(_context).Shuffle(),
                    RoundPlayers = new List<RoundPlayer>(),
                    RoundResults = new List<RoundResult>()
                };

                List<RoundPlayer> roundPlayers = new List<RoundPlayer>();
                if (lastRound == null)
                {
                    game.Status = GameStatus.Playing;
                    Player firstDealer = game.GamePlayers.First(u => u.InitialSeatWind == WindDirection.East).Player;
                    newRound.Wind = WindDirection.East;
                    newRound.RoundCounter = 1;
                    foreach (var gp in game.GamePlayers)
                    {
                        var rp = new RoundPlayer { GamePlayerId = gp.Id, GamePlayer = gp, Round = newRound, Wind = gp.InitialSeatWind.Value, Points = gp.Points };
                        if (gp.PlayerId == firstDealer.Id)
                        {
                            rp.IsInitialDealer = true;
                            rp.IsDealer = true;
                            rp.IsMyTurn = true;
                            rp.MustThrow = true;
                        }
                        roundPlayers.Add(rp);
                    }
                }
                else
                {
                    newRound.RoundCounter = lastRound.RoundCounter + 1;
                    var lastRoundDealer = lastRound.RoundPlayers.First(u => u.IsDealer);

                    //if this is not the first round
                    //last round check
                    //1.) if the winner of last round == dealer then no wind change
                    //2.) if last round "IsTied" set to true then no wind change
                    if (lastRound.IsTied)
                    {
                        newRound.Wind = lastRound.Wind;
                        roundPlayers.AddRange(GetNewUserRounds(lastRound.RoundPlayers, sameRound: true, lastRoundDealer.Wind));
                    }
                    else
                    {
                        //if last game is not tied, then there gotta be a winner here
                        //could be more than one winners here
                        var lastRoundWinners = lastRound.RoundResults.Where(x => x.PlayResult == PlayResult.Win);
                        var dealerWonLastRound = lastRoundWinners.Any(x => x.PlayerId == lastRoundDealer.GamePlayer.Player.Id);

                        if (dealerWonLastRound)
                        {
                            newRound.Wind = lastRound.Wind;
                            roundPlayers.AddRange(GetNewUserRounds(lastRound.RoundPlayers, sameRound: true, lastRoundDealer.Wind));
                        }
                        else
                        {
                            //determine nextdealer
                            var windOfNextDealer = NextWindClockWise(lastRoundDealer.Wind);
                            roundPlayers.AddRange(GetNewUserRounds(lastRound.RoundPlayers, sameRound: false, windOfNextDealer));
                            var roundWindChanged = roundPlayers.Any(p => p.IsDealer == true && p.IsInitialDealer == true);
                            newRound.Wind = roundWindChanged ? NextWindClockWise(lastRound.Wind) : lastRound.Wind;                            
                        }
                    }
                }

                foreach (var ur in roundPlayers)
                {
                    newRound.RoundPlayers.Add(ur);
                }

                var theDealer = roundPlayers.First(u => u.IsDealer);

                var dealerId = theDealer.GamePlayerId;

                //for debugging
                //RoundTileHelper.SetupForWinPongChowPriority(newRound.RoundTiles);

                //tiles assignment and sorting
                foreach (var gamePlayer in game.GamePlayers)
                {
                    if (gamePlayer.Id == dealerId)
                    {
                        RoundTileHelper.AssignTilesToUser(14, gamePlayer.Player.UserName, newRound.RoundTiles);
                        //set one tile status to be justpicked
                        newRound.RoundTiles.First(rt => rt.Owner == gamePlayer.Player.UserName && rt.Tile.TileType != TileType.Flower).Status = TileStatus.UserJustPicked;
                        var playerTiles = newRound.RoundTiles.Where(rt => rt.Owner == gamePlayer.Player.UserName && (rt.Status == TileStatus.UserActive || rt.Status == TileStatus.UserJustPicked)).ToList();
                        RoundTileHelper.AssignAliveTileCounter(playerTiles);
                    }
                    else
                    {
                        RoundTileHelper.AssignTilesToUser(13, gamePlayer.Player.UserName, newRound.RoundTiles);
                        var playerTiles = newRound.RoundTiles.Where(rt => rt.Owner == gamePlayer.Player.UserName && rt.Status == TileStatus.UserActive).ToList();
                        RoundTileHelper.AssignAliveTileCounter(playerTiles);
                    }
                }

                _context.Rounds.Add(newRound);

                var success = await _context.SaveChangesAsync() > 0;

                List<RoundDto> results = new List<RoundDto>();

                foreach (var p in newRound.RoundPlayers)
                {
                    results.Add(_mapper.Map<Round, RoundDto>(newRound, opt => opt.Items["MainRoundPlayer"] = p));
                }

                if (success) return results;

                throw new Exception("Problem creating a new round");
            }

            private IEnumerable<RoundPlayer> GetNewUserRounds(IEnumerable<RoundPlayer> userRounds, bool sameRound, WindDirection windOfDealer)
            {
                List<RoundPlayer> ret = new List<RoundPlayer>();

                foreach (var lur in userRounds)
                {
                    var userWind = sameRound ? lur.Wind : NextWindAntiClockwise(lur.Wind);
                    var ur = new RoundPlayer
                    {
                        GamePlayerId = lur.GamePlayerId,
                        GamePlayer = lur.GamePlayer,
                        IsInitialDealer = lur.IsInitialDealer,
                        IsDealer = lur.Wind == windOfDealer,
                        IsMyTurn = lur.Wind == windOfDealer,
                        MustThrow = lur.Wind == windOfDealer,
                        Points = lur.Points,
                        Wind = userWind
                    };
                    ret.Add(ur);
                }

                return ret;
            }

            private WindDirection NextWindAntiClockwise(WindDirection wind)
            {
                WindDirection ret;
                switch (wind)
                {
                    case WindDirection.East:
                        ret = WindDirection.North;
                        break;
                    case WindDirection.South:
                        ret = WindDirection.East;
                        break;
                    case WindDirection.West:
                        ret = WindDirection.South;
                        break;
                    case WindDirection.North:
                        ret = WindDirection.West;
                        break;
                    default:
                        ret = WindDirection.East;
                        break;
                }
                return ret;
            }

            private WindDirection NextWindClockWise(WindDirection wind)
            {
                WindDirection ret;
                switch (wind)
                {
                    case WindDirection.East:
                        ret = WindDirection.South;
                        break;
                    case WindDirection.South:
                        ret = WindDirection.West;
                        break;
                    case WindDirection.West:
                        ret = WindDirection.North;
                        break;
                    case WindDirection.North:
                        ret = WindDirection.East;
                        break;
                    default:
                        ret = WindDirection.East;
                        break;
                }
                return ret;
            }
        }
    }
}
