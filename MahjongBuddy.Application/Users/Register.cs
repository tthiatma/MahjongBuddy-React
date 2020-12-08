using FluentValidation;
using MahjongBuddy.Application.Errors;
using MahjongBuddy.Application.Interfaces;
using MahjongBuddy.Application.Validators;
using MahjongBuddy.Core;
using MahjongBuddy.EntityFramework.EntityFramework;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MahjongBuddy.Application.Users
{
    public class Register
    {
        public class Command : IRequest
        {
            public string DisplayName { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string Origin { get; set; }
        }

        public class CommandValidator: AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.DisplayName).NotEmpty();
                RuleFor(x => x.UserName).NotEmpty();
                RuleFor(x => x.Email).NotEmpty().EmailAddress();
                RuleFor(x => x.Password).Password();
            }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly MahjongBuddyDbContext _context;
            private readonly UserManager<Player> _userManager;
            private readonly IEmailSender _emailSender;
            private readonly IJwtGenerator _jwtGenerator;

            public Handler(MahjongBuddyDbContext context, UserManager<Player> userManager, IEmailSender emailSender, IJwtGenerator jwtGenerator)
            {
                _context = context;
                _userManager = userManager;
                _emailSender = emailSender;
                _jwtGenerator = jwtGenerator;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                if (await _context.Users.Where(u => u.Email == request.Email).AnyAsync())
                    throw new RestException(HttpStatusCode.BadRequest, new { Email = "Email already exist" });

                if (await _context.Users.Where(u => u.UserName == request.UserName).AnyAsync())
                    throw new RestException(HttpStatusCode.BadRequest, new { UserName = "UserName already exist" });

                var user = new Player
                {
                    DisplayName = request.DisplayName,
                    Email = request.Email,
                    UserName = request.UserName,
                    DateCreated = DateTime.Now
                };


                //var refreshToken = _jwtGenerator.GenerateRefreshToken();
                //user.RefreshTokens.Add(refreshToken);

                var result = await _userManager.CreateAsync(user, request.Password);
                if (!result.Succeeded)
                    throw new Exception("Problem creating user!");

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

                var verifyUrl = $"{request.Origin}/user/verifyEmail?token={token}&email={request.Email}";

                var message = $"<p>Please click the below link to verify your email address:</p><p><a href='{verifyUrl}'>{verifyUrl}</a></p>";

                await _emailSender.SendEmailAsync(request.Email, "Please verify email address", message);

                return Unit.Value;
            }
        }
    }
}
