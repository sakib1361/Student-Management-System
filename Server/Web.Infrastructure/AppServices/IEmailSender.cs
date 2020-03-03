using System.Threading.Tasks;

namespace Student.Infrastructure.AppServices
{
    public interface IEmailSender
    {
       void SendEmailAsync(string email, string subject, string message);
    }

    public class AuthMessageSenderOptions
    {
        public string SMTPServer { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string From { get; set; }
    }
}
