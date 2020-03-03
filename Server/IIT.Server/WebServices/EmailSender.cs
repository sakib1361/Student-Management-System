using Microsoft.Extensions.Options;
using Student.Infrastructure.AppServices;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace IIT.Server.WebServices
{
    public class EmailSender : IEmailSender
    {
        public AuthMessageSenderOptions Options { get; }
        private readonly SemaphoreSlim semaphoreSlim;

        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
            semaphoreSlim = new SemaphoreSlim(1, 1);
        }


        public async void SendEmailAsync(string email, string subject, string message)
        {
            await semaphoreSlim.WaitAsync();
            await Execute(email, subject, message);
            semaphoreSlim.Release();
        }

        private async Task<bool> Execute(string email, string subject, string message)
        {

            try
            {
                using (var client = new SmtpClient())
                {
                    var mail = new MailMessage(Options.From, email)
                    {
                        Subject = subject,
                        Body = message
                    };


                    client.Host = Options.SMTPServer;
                    client.Port = 587;
                    client.EnableSsl = true;
                    client.Timeout = 10000;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(Options.Username, Options.Password);
#if DEBUG
                Console.WriteLine("Email Demo: " + email);
                await Task.Delay(100);
#else     
                await client.SendMailAsync(mail);
#endif
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
