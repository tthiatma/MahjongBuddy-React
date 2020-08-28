using System.Threading.Tasks;

namespace MahjongBuddy.Application.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string userEmail, string emailSubject, string message);
    }
}
