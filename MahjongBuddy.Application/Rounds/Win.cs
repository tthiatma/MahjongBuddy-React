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

                var winner = round.UserRounds.FirstOrDefault(u => u.AppUser.UserName == request.UserName);

                if (winner == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Player = "Could not find player" });

                //var ut = round.RoundTiles.Where(t => t.Owner == request.UserName).Select(t=> t.Id).ToList();
                //var rawr = _context.RoundTiles.Where(t => t.Owner == request.UserName).Select(t => t.Id).ToList();
                //var difference = rawr.Except(ut);
                //var theWeirdOne = _context.RoundTiles.First(t => t.Id == difference.First());

                //if its a valid win:
                HandWorth handWorth = _pointCalculator.Calculate(round, request.UserName);

                if(handWorth == null)
                    throw new RestException(HttpStatusCode.BadRequest, new { Win = "Invalid combination hand" });

                if (handWorth.Points >= game.MinPoint)
                {
                    //set the game as over
                    round.IsOver = true;
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

                    round.RoundResults.Add(winnerResult);

                    //now that we have the winner, check the loser

                    if(isSelfPick)
                    {
                        //if its self pick, then all 3 other players needs to record the loss
                        var losers = round.UserRounds.Where(u => u.AppUser.UserName != request.UserName);

                        foreach (var l in losers)
                        {
                            round.RoundResults.Add(new RoundResult { IsWinner = false, AppUser = l.AppUser });
                        }
                    }
                    else
                    {
                        //otherwise there is only one loser that throw the tile to board
                        var boardTile = round.RoundTiles.First(t => t.Owner == "Board");
                        var loser = round.UserRounds.First(u => u.AppUser.UserName == boardTile.ThrownBy);
                        round.RoundResults.Add(new RoundResult { IsWinner = false, AppUser = loser.AppUser});
                    }

                    //TODO implement more than one winner
                    
                    var roundToReturn = _mapper.Map<Round, RoundDto>(round);

                    var success = await _context.SaveChangesAsync() > 0;

                    if (success)
                        return roundToReturn;

                }
                else
                    throw new RestException(HttpStatusCode.BadRequest, new {Win = "Not enough point to win with this hand" });

                throw new Exception("Problem calling win");
            }
        }
    }
}