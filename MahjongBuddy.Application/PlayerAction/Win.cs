using AutoMapper;
using MahjongBuddy.Application.Dtos;
using MahjongBuddy.Application.Errors;
using MahjongBuddy.Application.Interfaces;
using MahjongBuddy.Application.Rounds.Scorings;
using MahjongBuddy.Core;
using MahjongBuddy.Core.Enums;
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
    public class Win
    {
        public class Command : IRequest<IEnumerable<RoundDto>>
        {
            public string GameCode { get; set; }
            public int RoundId { get; set; }
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
                var game = await _context.Games.FirstOrDefaultAsync(x => x.Code == request.GameCode.ToUpper());
                if (game == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Game = "Could not find game" });

                var round = game.Rounds.FirstOrDefault(r => r.Id == request.RoundId);
                if (round == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Round = "Could not find round" });

                RoundPlayer roundPlayerWinner = round.RoundPlayers.FirstOrDefault(u => u.GamePlayer.Player.UserName == request.UserName);

                if (roundPlayerWinner == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Player = "Could not find player" });

                //check for valid win:
                HandWorth handWorth = _pointCalculator.Calculate(round, request.UserName);
                if (handWorth == null)
                    throw new RestException(HttpStatusCode.BadRequest, new { Win = "Invalid combination hand" });

                if (handWorth.Points >= game.MinPoint)
                {
                    var winAction = roundPlayerWinner.RoundPlayerActions.FirstOrDefault(a => a.ActionType == ActionType.Win);
                    if (winAction != null) winAction.ActionStatus = ActionStatus.Activated;

                    bool isSelfPick = false;

                    //set the game as over if all win action is settled
                    var playerWithUnsettledWin = round.RoundPlayers.Where(p => p.RoundPlayerActions.Any(pa => pa.ActionType == ActionType.Win && pa.ActionStatus == ActionStatus.Active));

                    if(playerWithUnsettledWin.Count() == 0)
                    {
                        round.IsOver = true;
                        round.IsEnding = false;
                    }

                    //create the result and record who win and who lost 
                    RoundResult winnerResult = new RoundResult
                    {
                        Player = roundPlayerWinner.GamePlayer.Player,
                        PlayResult = PlayResult.Win,
                    };

                    if (round.RoundResults == null)
                        round.RoundResults = new List<RoundResult>();

                    //record hand type and extra points
                    foreach (var h in handWorth.HandTypes)
                    {
                        var point = _pointCalculator.HandTypeLookup[h];
                        winnerResult.RoundResultHands.Add(new RoundResultHand { HandType = h, Point = point, Name = h.ToString() });
                    }

                    foreach (var e in handWorth.ExtraPoints)
                    {
                        if (e == ExtraPoint.SelfPick)
                            isSelfPick = true;

                        var point = _pointCalculator.ExtraPointLookup[e];
                        winnerResult.RoundResultExtraPoints.Add(new RoundResultExtraPoint { ExtraPoint = e, Point = point, Name = e.ToString() });
                    }

                    //now that we have the winner hand type and extra point recorded, let's calculate the points

                    //if the handworth exceed game max point, cap the point to game's max point 
                    var cappedPoint = handWorth.Points > game.MaxPoint ? game.MaxPoint : handWorth.Points;
                    var losingPoint = cappedPoint * -1;

                    if (isSelfPick)
                    {
                        //check if "bao"
                        //if there is AllOneSuit or SmallDragon or BigDragon or smallFourWind or bigFourWind
                        //then the one that "bao" will be the only one that pays to the winner 

                        bool isLoserBao = false;
                        string baoPlayerUserName = string.Empty;
                        //check for allonesuit
                        var winnerTiles = round.RoundTiles.Where(t => t.Owner == roundPlayerWinner.GamePlayer.Player.UserName);
                        if (handWorth.HandTypes.Contains(HandType.AllOneSuit) 
                            || handWorth.HandTypes.Contains(HandType.SmallFourWind)
                            || handWorth.HandTypes.Contains(HandType.BigFourWind))
                        {
                            //check if the 4th tilesetgroupindex has thrownby value 
                            var fourthGroupTileIndex = winnerTiles.FirstOrDefault(t => t.TileSetGroupIndex == 4 && !string.IsNullOrEmpty(t.ThrownBy));
                            if (fourthGroupTileIndex != null)
                            {
                                isLoserBao = true;
                                baoPlayerUserName = fourthGroupTileIndex.ThrownBy;
                            }
                        }

                        //check for dragon
                        if (handWorth.HandTypes.Contains(HandType.SmallDragon) || handWorth.HandTypes.Contains(HandType.BigDragon))
                        {
                            //find the index of first pong dragon
                            var pongOrKongDragons = winnerTiles.Where(t => (t.TileSetGroup == TileSetGroup.Pong || t.TileSetGroup == TileSetGroup.Kong) 
                            && t.Tile.TileType == TileType.Dragon && !string.IsNullOrEmpty(t.ThrownBy));

                            //if there is 3rd set of dragon pong/kong, then its not a bao 
                            //weird rule ever
                            //then find the index of second pong dragon and check thrown by
                            if (pongOrKongDragons.Count() == 2)
                            {
                                isLoserBao = true;
                                baoPlayerUserName = pongOrKongDragons.OrderBy(t => t.TileSetGroupIndex).Last().ThrownBy;
                            }
                        }

                        if (isLoserBao)
                        {
                            //the loser that bao will pay the winning point times three
                            var winningPoint = cappedPoint * 3;
                            roundPlayerWinner.Points += winningPoint;                            
                            winnerResult.Points = winningPoint;

                            RoundPlayer roundPlayerloser = round.RoundPlayers.FirstOrDefault(p => p.GamePlayer.Player.UserName == baoPlayerUserName);
                            roundPlayerloser.Points -= winningPoint;
                            round.RoundResults.Add(new RoundResult { PlayResult = PlayResult.LostWithPenalty, Player = roundPlayerloser.GamePlayer.Player, Points = losingPoint * 3 });

                            //record users that are tied
                            var tiedPlayers = round.RoundPlayers.Where(p => p.GamePlayer.Player.UserName != baoPlayerUserName && p.GamePlayer.Player.UserName != roundPlayerWinner.GamePlayer.Player.UserName);
                            tiedPlayers.ForEach(tp =>
                            {
                                round.RoundResults.Add(new RoundResult { PlayResult = PlayResult.Tie, Player = tp.GamePlayer.Player, Points = 0 });
                            });
                        }
                        else
                        {
                            //if its self pick, and no bao, then all 3 other players needs to record the loss
                            var losers = round.RoundPlayers.Where(u => u.GamePlayer.Player.UserName != request.UserName);

                            //points will be times 3
                            var winningPoint = cappedPoint * 3;
                            roundPlayerWinner.Points += winningPoint;
                            winnerResult.Points = winningPoint;

                            losers.ForEach(l =>
                            {
                                l.Points -= cappedPoint;
                                round.RoundResults.Add(new RoundResult { PlayResult = PlayResult.Lost, Player = l.GamePlayer.Player, Points = losingPoint });
                            });
                        }
                    }
                    else
                    {
                        //otherwise there is only one loser that throw the tile to board
                        roundPlayerWinner.Points += cappedPoint;
                        winnerResult.Points = cappedPoint;

                        var boardTile = round.RoundTiles.First(t => t.Owner == DefaultValue.board && t.Status == TileStatus.BoardActive);
                        var loser = round.RoundPlayers.First(u => u.GamePlayer.Player.UserName == boardTile.ThrownBy);
                        loser.Points -= cappedPoint;
                        round.RoundResults.Add(new RoundResult { PlayResult = PlayResult.Lost, Player = loser.GamePlayer.Player, Points = losingPoint });

                        //check for multiple winners. If player has a valid win and already recorded as tie
                        var tieResultCouldWin = round.RoundResults.FirstOrDefault(rr => rr.PlayResult == PlayResult.Tie && rr.Player.UserName == roundPlayerWinner.GamePlayer.Player.UserName);

                        if (tieResultCouldWin != null)
                        {
                            //remove record that the player tie
                            round.RoundResults.Remove(tieResultCouldWin);
                        }
                        else
                        {
                            //record users that are tied
                            var tiedPlayers = round.RoundPlayers.Where(p => p.GamePlayer.Player.UserName != loser.GamePlayer.Player.UserName && p.GamePlayer.Player.UserName != roundPlayerWinner.GamePlayer.Player.UserName);
                            tiedPlayers.ForEach(tp =>
                            {
                                round.RoundResults.Add(new RoundResult { PlayResult = PlayResult.Tie, Player = tp.GamePlayer.Player, Points = 0 });
                            });
                        }

                    }
                    round.RoundResults.Add(winnerResult);

                    //tally the point in gameplayer
                    round.RoundPlayers.ForEach(rp =>
                    {
                        rp.GamePlayer.Points = rp.Points;
                    });

                    var success = await _context.SaveChangesAsync() > 0;

                    if (success)
                    {
                        List<RoundDto> results = new List<RoundDto>();

                        foreach (var p in round.RoundPlayers)
                        {
                            results.Add(_mapper.Map<Round, RoundDto>(round, opt => opt.Items["MainRoundPlayer"] = p));
                        }
                        return results;
                    }
                }
                else
                    throw new RestException(HttpStatusCode.BadRequest, new { Win = "Not enough point to win with this hand" });

                throw new Exception("Problem calling win");
            }
        }
    }
}