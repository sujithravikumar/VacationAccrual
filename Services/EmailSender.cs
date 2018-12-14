using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;

namespace vacation_accrual_buddy
{
    public class EmailSender : IEmailSender
    {
        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        public AuthMessageSenderOptions Options { get; } //set only via Secret Manager

        public Task SendEmailAsync(string email, string subject, string body)
        {
            return Execute(Options.Smtp_Host, Options.Smtp_Port,
                Options.Smtp_Username, Options.Smtp_Password, subject, body, email);
        }

        public async Task Execute(string host, int port, string username,
            string password, string subject, string body, string email)
        {

            MailMessage message = new MailMessage();
            message.IsBodyHtml = true;
            message.From = new MailAddress("vacation.accrual.buddy@gmail.com", "Vacation Accrual Buddy");
            message.To.Add(new MailAddress(email));
            message.Subject = subject;
            message.Body = body;

            using (var client = new SmtpClient(host, port))
            {
                // FIXME: Uncomment the following before releasing to production

                // Pass SMTP credentials
                //client.Credentials =
                //new NetworkCredential(username, password);

                // Enable SSL encryption
                //client.EnableSsl = true;

                try
                {
                    await client.SendMailAsync(message);
                }
                catch
                {
                    // TODO: Add some logging
                    throw;
                }
            }
        }
    }
}
