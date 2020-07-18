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

namespace MahjongBuddy.Application.Rounds
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
                var game = await _context.Games.FindAsync(request.GameId);
                var round = await _context.Rounds.FindAsync(request.RoundId);
                if (game == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Game = "Could not find game" });

                if (round == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Round = "Could not find round" });

                var winner = round.RoundPlayers.FirstOrDefault(u => u.AppUser.UserName == request.UserName);

                if (winner == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Player = "Could not find player" });

                //if its a valid win:
                HandWorth handWorth = _pointCalculator.Calculate(round, request.UserName);

                if(handWorth == null)
                    throw new RestException(HttpStatusCode.BadRequest, new { Win = "Invalid combination hand" });

                if (handWorth.Points >= game.MinPoint)
                {
                    //set the game as over
                    round.IsOver = true;
                    round.IsEnding = false;
                    //create the result
                    //record who win and who lost 
                    RoundResult winnerResult = new RoundResult
                    {
                        AppUser = winner.AppUser,
                        IsWinner = true,                                                
                    };

                    if (round.RoundResults == null)
                        round.RoundResults = new List<RoundResult>();

                    bool isSelfPick = false;

                    //record hand type and extra points
                    foreach (var h in handWorth.HandTypes)
                    {
                        var point = _pointCalculator.HandTypeLookup[h];
                        winnerResult.RoundResultHands.Add(new RoundResultHand {HandType = h, Point = point, Name = h.ToString() });
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
                        //if its self pick, then all 3 other players needs to record the loss
                        var losers = round.RoundPlayers.Where(u => u.AppUser.UserName != request.UserName);

                        //points will be times 3
                        var winningPoint = cappedPoint * 3;
                        winner.Points += winningPoint;
                        winnerResult.PointsResult = winningPoint;

                        foreach (var l in losers)
                        {
                            l.Points -= cappedPoint;
                            round.RoundResults.Add(new RoundResult { IsWinner = false, AppUser = l.AppUser, PointsResult = losingPoint });
                        }
                    }
                    else
                    {
                        //otherwise there is only one loser that throw the tile to board
                        winner.Points += cappedPoint;
                        winnerResult.PointsResult = cappedPoint;

                        var boardTile = round.RoundTiles.First(t => t.Owner == DefaultValue.board && t.Status == TileStatus.BoardActive);
                        var loser = round.RoundPlayers.First(u => u.AppUser.UserName == boardTile.ThrownBy);
                        loser.Points -= cappedPoint;
                        round.RoundResults.Add(new RoundResult { IsWinner = false, AppUser = loser.AppUser, PointsResult = losingPoint });
                    }
                    round.RoundResults.Add(winnerResult);

                    //TODO implement more than one winner

                    var success = await _context.SaveChangesAsync() > 0;

                    if (success)
                    {
                        var roundToReturn = _mapper.Map<Round, RoundDto>(round);
                        return roundToReturn;
                    }

                }
                else
                    throw new RestException(HttpStatusCode.BadRequest, new {Win = "Not enough point to win with this hand" });

                throw new Exception("Problem calling win");
            }
        }
    }
}