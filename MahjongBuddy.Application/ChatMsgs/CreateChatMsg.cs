using AutoMapper;
using MahjongBuddy.Application.Dtos;
using MahjongBuddy.Application.Errors;
using MahjongBuddy.Core;
using MahjongBuddy.EntityFramework.EntityFramework;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace MahjongBuddy.Application.ChatMsgs
{
    public class CreateChatMsg
    {
        public class Command : IRequest<ChatMsgDto>
        {
            public string Body { get; set; }
            public string GameCode { get; set; }
            public string UserName { get; set; }
        }
        public class Handler : IRequestHandler<Command, ChatMsgDto>
        {
            private readonly MahjongBuddyDbContext _context;
            private readonly IMapper _mapper;

            public Handler(MahjongBuddyDbContext context,IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<ChatMsgDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var game = await _context.Games.FirstOrDefaultAsync(g => g.Code == request.GameCode.ToUpper());

                if (game == null)
                    throw new RestException(HttpStatusCode.NotFound, new {Game = "Game not found" });

                var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == request.UserName);

                var chatMsg = new ChatMsg
                {
                    Author = user,
                    Game = game,
                    Body = request.Body,
                    CreatedAt = DateTime.Now
                };

                game.ChatMsgs.Add(chatMsg);

                var success = await _context.SaveChangesAsync() > 0;

                if (success) return _mapper.Map<ChatMsgDto>(chatMsg);

                throw new Exception("Problem saving changes");
            }
        }
    }
}
