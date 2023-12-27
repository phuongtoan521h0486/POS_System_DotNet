using MimeKit;
using MailKit.Net.Smtp;

namespace POS_System_DotNet.Services
{
    public interface IEmailService
    {
        void SendEmail(string to, string subject, string body);
    }

    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendEmail(string to, string subject, string body)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("GTV POS", "gtvprimeofficial@gmail.com"));
            emailMessage.To.Add(new MailboxAddress("User", to));
            emailMessage.Subject = subject;

            var textPart = new TextPart("plain")
            {
                Text = body
            };


            emailMessage.Body = textPart;

            using (var client = new SmtpClient())
            {
                client.Connect(
                    "smtp.gmail.com",
                    587,
                    false
                );

                client.Authenticate(
                    "gtvprimeofficial@gmail.com",
                    "jihfblsarmerterk"
                );

                client.Send(emailMessage);
                client.Disconnect(true);
            }
        }
    }
}
