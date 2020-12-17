using AutoMapper;
using MahjongBuddy.Application.Dtos;
using MahjongBuddy.Application.Interfaces;
using MahjongBuddy.Core;
using MahjongBuddy.EntityFramework.EntityFramework;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MahjongBuddy.Application.Games
{
    public class List
    {
        public class GamesEnvelope
        {
            public List<GameDto> Games { get; set; }
            public int GameCount { get; set; }
        }

        public class Query : IRequest<GamesEnvelope> 
        {
            public Query(int? limit, int? offset, bool isInGame, bool isHost, DateTime? startDate)
            {
                Limit = limit;
                Offset = offset;
                IsHost = isHost;
                IsInGame = isInGame;
                StartDate = startDate ?? DateTime.Now.AddDays(-3);
            }

            public int? Limit { get; set; }
            public int? Offset { get; set; }
            public bool IsHost { get; set; }
            public bool IsInGame { get; set; }
            public DateTime? StartDate { get; set; }
        }

        public class Handler : IRequestHandler<Query, GamesEnvelope>
        {
            private readonly MahjongBuddyDbContext _context;
            private readonly IMapper _mapper;
            private readonly IUserAccessor _userAccessor;

            public Handler(MahjongBuddyDbContext context, IMapper mapper, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _context = context;
                _mapper = mapper;
            }
            public async Task<GamesEnvelope> Handle(Query request, CancellationToken cancellationToken)
            {
                var queryable = _context.Games
                   .Where(x => x.Date >= request.StartDate)
                   .OrderBy(x => x.Date)
                   .AsQueryable();

                if (request.IsInGame)
                {
                    queryable = queryable.Where(x => x.GamePlayers.Any(a => a.Player.UserName == _userAccessor.GetCurrentUserName()));
                }

                if (request.IsHost)
                {
                    queryable = queryable.Where(x => x.GamePlayers.Any(a => a.Player.UserName == _userAccessor.GetCurrentUserName() && a.IsHost));
                }

                var games = await queryable
                    .Skip(request.Offset ?? 0)
                    .Take(request.Limit ?? 10).ToListAsync();

                return new GamesEnvelope
                {
                    Games = _mapper.Map<List<Game>, List<GameDto>>(games),
                    GameCount = queryable.Count()
                };
            }
        }
    }
}
