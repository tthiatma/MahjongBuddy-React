using MahjongBuddy.Application.Interfaces;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace MahjongBuddy.Infrastructure.Email
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly IOptions<SmtpSettings> _settings;

        public SmtpEmailSender(IOptions<SmtpSettings> settings)
        {
            _settings = settings;
        }
        public async Task SendEmailAsync(string userEmail, string emailSubject, string message)
        {
            //create the mail message 
            MailMessage mail = new MailMessage();

            //set the addresses 
            mail.From = new MailAddress(_settings.Value.FromAddress);
            mail.To.Add(userEmail);

            //set the content 
            mail.Subject = emailSubject;
            mail.Body = message;
            mail.IsBodyHtml = true;

            //send the message 
            using (var smtpClient = new SmtpClient(_settings.Value.Server, _settings.Value.Port))
            {
                NetworkCredential Credentials = new NetworkCredential(_settings.Value.FromAddress, _settings.Value.Password);
                smtpClient.Credentials = Credentials;
                await smtpClient.SendMailAsync(mail);
            }
        }
    }
}
