using MahjongBuddy.Application.Interfaces;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace MahjongBuddy.Infrastructure.Email
{
    public class SendGridEmailSender : IEmailSender
    {
        private readonly IOptions<SendGridSettings> _settings;

        public SendGridEmailSender(IOptions<SendGridSettings> settings)
        {
            _settings = settings;
        }
        public async Task SendEmailAsync(string userEmail, string emailSubject, string message)
        {
            var client = new SendGridClient(_settings.Value.ApiKey);
            var msg = new SendGridMessage
            {
                From = new EmailAddress("info@mahjongbuddy.com", _settings.Value.User),
                Subject = emailSubject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(userEmail));
            msg.SetClickTracking(false, false);

            await client.SendEmailAsync(msg);
        }
    }
}
