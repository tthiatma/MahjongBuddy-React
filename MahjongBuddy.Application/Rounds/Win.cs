using AutoMapper;
using MahjongBuddy.Application.Dtos;
using MahjongBuddy.Application.Errors;
using MahjongBuddy.Application.Interfaces;
using MahjongBuddy.Application.Rounds.Scorings;
using MahjongBuddy.Core;
using MahjongBuddy.EntityFramework.EntityFramework;
using MediatR;
using System;
using System.Linq;
using System.Net;
using System.Runtime.ExceptionServices;
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

                if (round == null || game == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Round = "Could not find round" });

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
                    RoundResult result = new RoundResult();
                    //record who win and who lost 

                    round.RoundResults.Add(result);

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