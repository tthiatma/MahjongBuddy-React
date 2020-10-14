using MahjongBuddy.Application.Errors;
using MahjongBuddy.Application.Interfaces;
using MahjongBuddy.Core;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MahjongBuddy.Application.Users
{
    public class ResendForgotPassword
    {
        public class Query : IRequest
        {
            public string Email { get; set; }

            public string Origin { get; set; }
        }

        public class Handler : IRequestHandler<Query>
        {
            private readonly UserManager<AppUser> _userManager;
            private readonly IEmailSender _emailSender;

            public Handler(UserManager<AppUser> userManager, IEmailSender emailSender)
            {
                _userManager = userManager;
                _emailSender = emailSender;
            }
            public async Task<Unit> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByEmailAsync(request.Email);

                if (user == null)
                    throw new RestException(HttpStatusCode.Unauthorized);

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
