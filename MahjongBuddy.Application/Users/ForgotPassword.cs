using FluentValidation;
using MahjongBuddy.Application.Interfaces;
using MahjongBuddy.Core;
using MahjongBuddy.EntityFramework.EntityFramework;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MahjongBuddy.Application.Users
{
    public class ForgotPassword
    {
        public class Command : IRequest
        {
            public string Email { get; set; }
            public string Origin { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Email).NotEmpty().EmailAddress();
            }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly MahjongBuddyDbContext _context;
            private readonly UserManager<Player> _userManager;
            private readonly IEmailSender _emailSender;

            public Handler(MahjongBuddyDbContext context, UserManager<Player> userManager, IEmailSender emailSender)
            {
                _context = context;
                _userManager = userManager;
                _emailSender = emailSender;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                //don't even try to do anything if user doesn't exist
                var userExist = await _context.Users.Where(u => u.Email == request.Email).AnyAsync();
                if (!userExist)
                    return Unit.Value;

                var user = await _userManager.FindByEmailAsync(request.Email);

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

                var verifyUrl = $"{request.Origin}/user/resetPassword?token={token}&email={request.Email}";

                var message = $"<p>Please click the below link to reset your password:</p><p><a href='{verifyUrl}'>{verifyUrl}</a></p>";

                await _emailSender.SendEmailAsync(request.Email, "Password Reset", message);

                return Unit.Value;
            }
        }
    }
}
