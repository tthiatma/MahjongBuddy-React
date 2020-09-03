using MediatR;
using System.Threading.Tasks;
using System.Threading;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using System.Net;
using MahjongBuddy.Application.Errors;
using MahjongBuddy.Application.Interfaces;
using MahjongBuddy.Core;
using System;
using System.Linq;

namespace MahjongBuddy.Application.Users
{
    public class Login
    {
        public class Query : IRequest<User> 
        {
            public string Email { get; set; }

            public string Password { get; set; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Query, User>
        {
            private readonly UserManager<AppUser> _userManager;
            private readonly SignInManager<AppUser> _signInManager;
            private readonly IJwtGenerator _jwtGenerator;
            public Handler(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IJwtGenerator jwtGenerator)
            {
                _userManager = userManager;
                _signInManager = signInManager;
                _jwtGenerator = jwtGenerator;
            }

            public async Task<User> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByEmailAsync(request.Email);

                if (user == null)
                    throw new RestException(HttpStatusCode.Unauthorized);

                if (!user.EmailConfirmed)
                    throw new RestException(HttpStatusCode.BadRequest, new { Email = "Email is not confirmed" });

                var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

                if(result.Succeeded)
                {
                    user.RefreshToken = _jwtGenerator.GenerateRefreshToken();
                    user.RefreshTokenExpiry = DateTime.Now.AddDays(30);

                    await _userManager.UpdateAsync(user);

                    return new User
                    { 
                        DisplayName = user.DisplayName,
                        Token = _jwtGenerator.CreateToken(user),
                        RefreshToken = user.RefreshToken,
                        UserName = user.UserName,
                        Image = user.Photos.FirstOrDefault(x => x.IsMain)?.Url
                    };                            
                }

                throw new RestException(HttpStatusCode.Unauthorized);
            }
        }
    }
}
