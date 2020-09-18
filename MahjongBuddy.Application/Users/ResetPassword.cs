using FluentValidation;
using MahjongBuddy.Application.Validators;
using MahjongBuddy.Core;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MahjongBuddy.Application.Users
{
    public class ResetPassword
    {
        public class Command : IRequest<IdentityResult>
        {
            public string Token { get; set; }
            public string Email { get; set; }
            public string NewPassword { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Token).NotEmpty();
                RuleFor(x => x.NewPassword).Password();
            }
        }

        public class Handler : IRequestHandler<Command, IdentityResult>
        {
            private readonly UserManager<AppUser> _userManager;

            public Handler(UserManager<AppUser> userManager)
            {
                _userManager = userManager;
            }
            public async Task<IdentityResult> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                var decodedTokenBytes = WebEncoders.Base64UrlDecode(request.Token);
                var decodedToken = Encoding.UTF8.GetString(decodedTokenBytes);
                return await _userManager.ResetPasswordAsync(user, decodedToken, request.NewPassword);
            }
        }
    }
}
