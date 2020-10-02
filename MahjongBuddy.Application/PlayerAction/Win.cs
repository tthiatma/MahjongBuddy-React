using AutoMapper;
using MahjongBuddy.Application.Dtos;
using MahjongBuddy.Application.Errors;
using MahjongBuddy.Application.Interfaces;
using MahjongBuddy.Application.Rounds.Scorings;
using MahjongBuddy.Core;
using MahjongBuddy.Core.Enums;
using MahjongBuddy.EntityFramework.EntityFramework;
using MediatR;
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
        public class Command : IRequest<RoundDto>
        {
            public int GameId { get; set; }
            public int RoundId { get; set; }
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
                //TODO implement more than one winner

                var game = await _context.Games.FindAsync(request.GameId);
                if (game == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Game = "Could not find game" });

                var round = game.Rounds.FirstOrDefault(r => r.Id == request.RoundId);
                if (round == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Round = "Could not find round" });

                var winCaller = round.RoundPlayers.FirstOrDefault(u => u.AppUser.UserName == request.UserName);

                if (winCaller == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Player = "Could not find player" });

                //if its a valid win:
                HandWorth handWorth = _pointCalculator.Calculate(round, request.UserName);

                if (handWorth == null)
                    throw new RestException(HttpStatusCode.BadRequest, new { Win = "Invalid combination hand" });

                if (handWorth.Points >= game.MinPoint)
                {
                    //set the game as over
                    round.IsOver = true;
                    round.IsEnding = false;
                    //create the result
                    //record who win and who lost 
                    RoundResult winnerRoundResult = new RoundResult
                    {
                        AppUser = winCaller.AppUser,
                        PlayerResult = PlayerResult.Win,
                    };

                    if (round.RoundResults == null)
                        round.RoundResults = new List<RoundResult>();

                    bool isSelfPick = false;

                    //record hand type and extra points
                    foreach (var h in handWorth.HandTypes)
                    {
                        var point = _pointCalculator.HandTypeLookup[h];
                        winnerRoundResult.RoundResultHands.Add(new RoundResultHand { HandType = h, Point = point, Name = h.ToString() });
                    }

                    foreach (var e in handWorth.ExtraPoints)
                    {
                        if (e == ExtraPoint.SelfPick)
                            isSelfPick = true;

                        var point = _pointCalculator.ExtraPointLookup[e];
                        winnerRoundResult.RoundResultExtraPoints.Add(new RoundResultExtraPoint { ExtraPoint = e, Point = point, Name = e.ToString() });
                    }

                    //now that we have the winner hand type and extra point recorded, let's calculate the points depending on min and max pts

                    //if the handworth exceed game max point, cap the point to game's max point 
                    var cappedPoint = handWorth.Points > game.MaxPoint ? game.MaxPoint : handWorth.Points;
                    var losingPoint = cappedPoint * -1;

                    if (isSelfPick)
                    {
                        //check if "bao"
                        var baoPlayerUserName = getBaoPlayerUserName(round, winCaller, handWorth);
                        if (!string.IsNullOrEmpty(baoPlayerUserName))
                        {
                            //the loser that bao will pay the winning point times three
                            var winningPoint = cappedPoint * 3;
                            winCaller.Points += winningPoint;
                            winnerRoundResult.PointsResult = winningPoint;

                            var loser = round.RoundPlayers.FirstOrDefault(p => p.AppUser.UserName == baoPlayerUserName);
                            loser.Points -= winningPoint;

                            round.RoundResults.Add(new RoundResult { PlayerResult = PlayerResult.Lost, AppUser = loser.AppUser, PointsResult = losingPoint * 3 });

                            //the other two players are tied
                            var tiedPlayers = round.RoundPlayers.Where(p => p.AppUser.UserName != winCaller.AppUser.UserName ).Where(p => p.AppUser.UserName != loser.AppUser.UserName);
                            foreach (var tiedPlayer in tiedPlayers)
                            {
                                round.RoundResults.Add(new RoundResult { PlayerResult = PlayerResult.Tie, AppUser = tiedPlayer.AppUser, PointsResult = 0 });
                            }
                        }
                        else
                        {
                            //if its self pick, and no bao, then all 3 other players needs to record the loss
                            var losers = round.RoundPlayers.Where(u => u.AppUser.UserName != request.UserName);

                            //points will be times 3
                            var winningPoint = cappedPoint * 3;
                            winCaller.Points += winningPoint;
                            winnerRoundResult.PointsResult = winningPoint;

                            foreach (var l in losers)
                            {
                                l.Points -= cappedPoint;
                                round.RoundResults.Add(new RoundResult { PlayerResult = PlayerResult.Lost, AppUser = l.AppUser, PointsResult = losingPoint });
                            }
                        }
                    }
                    else
                    {
                        //otherwise there is only one loser that throw the tile to board
                        winCaller.Points += cappedPoint;
                        winnerRoundResult.PointsResult = cappedPoint;

                        var boardTile = round.RoundTiles.First(t => t.Owner == DefaultValue.board && t.Status == TileStatus.BoardActive);
                        var loser = round.RoundPlayers.First(u => u.AppUser.UserName == boardTile.ThrownBy);
                        loser.Points -= cappedPoint;
                        round.RoundResults.Add(new RoundResult { PlayerResult = PlayerResult.Lost, AppUser = loser.AppUser, PointsResult = losingPoint });

                        //the other two players are tied
                        var tiedPlayers = round.RoundPlayers.Where(p => p.AppUser.UserName != winCaller.AppUser.UserName).Where(p => p.AppUser.UserName != loser.AppUser.UserName);
                        foreach (var tiedPlayer in tiedPlayers)
                        {
                            round.RoundResults.Add(new RoundResult { PlayerResult = PlayerResult.Tie, AppUser = tiedPlayer.AppUser, PointsResult = 0 });
                        }
                    }
                    round.RoundResults.Add(winnerRoundResult);

                    var success = await _context.SaveChangesAsync() > 0;

                    if (success)
                    {
                        var roundToReturn = _mapper.Map<Round, RoundDto>(round);
                        return roundToReturn;
                    }
                }
                else
                    throw new RestException(HttpStatusCode.BadRequest, new { Win = "Not enough point to win with this hand" });

                throw new Exception("Problem calling win");
            }

            /// <summary>
            /// if return username, that means this user has bao, otherwise return empty string if no bao
            /// </summary>
            /// <returns></returns>
            private string getBaoPlayerUserName(Round round, RoundPlayer winCaller, HandWorth handWorth)
            {
                //if there is AllOneSuit or SmallDragon or BigDragon or smallFourWind or bigFourWind
                //then the one that "bao" will be the only one that pays to the winner 

                string baoPlayerUserName = string.Empty;
                //check for allonesuit
                var winnerTiles = round.RoundTiles.Where(t => t.Owner == winCaller.AppUser.UserName);
                if (handWorth.HandTypes.Contains(HandType.AllOneSuit)
                    || handWorth.HandTypes.Contains(HandType.SmallFourWind)
                    || handWorth.HandTypes.Contains(HandType.BigFourWind))
                {
                    //check if the 4th tilesetgroupindex has thrownby value 
                    var fourthGroupTileIndex = winnerTiles.FirstOrDefault(t => t.TileSetGroupIndex == 4 && !string.IsNullOrEmpty(t.ThrownBy));

                    if (fourthGroupTileIndex != null)
                        baoPlayerUserName = fourthGroupTileIndex.ThrownBy;
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
                        baoPlayerUserName = pongOrKongDragons.OrderBy(t => t.TileSetGroupIndex).Last().ThrownBy;
                }

                return baoPlayerUserName;
            }
        }
    }
}