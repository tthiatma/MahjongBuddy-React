using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore.Identity;
using System.Net;
using MahjongBuddy.Application.Errors;
using MahjongBuddy.Application.Interfaces;
using MahjongBuddy.Core;
using System;
using System.Linq;

namespace MahjongBuddy.Application.Users
{
    public class RefreshToken
    {
        public class Command : IRequest<User> 
        {
            public string RefreshToken { get; set; }
        }

        public class Handler : IRequestHandler<Command, User>
        {
            private readonly UserManager<Player> _userManager;
            private readonly IJwtGenerator _jwtGenerator;
            private readonly IUserAccessor _userAccessor;


            public Handler(UserManager<Player> userManager, IJwtGenerator jwtGenerator, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _userManager = userManager;
                _jwtGenerator = jwtGenerator;
            }

            public async Task<User> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByNameAsync(_userAccessor.GetCurrentUserName());

                var oldToken = user.RefreshTokens.SingleOrDefault(x => x.Token == request.RefreshToken);

                if (oldToken != null && !oldToken.IsActive) throw new RestException(HttpStatusCode.Unauthorized);

                if (oldToken != null)
                {
                    oldToken.Revoked = DateTime.UtcNow;
                }

                var newRefreshToken = _jwtGenerator.GenerateRefreshToken();
                user.RefreshTokens.Add(newRefreshToken);

                await _userManager.UpdateAsync(user);

                return new User(user, _jwtGenerator, newRefreshToken.Token);
            }
        }
    }
}
