using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;
using System.Threading.Tasks;

namespace Veterinary.Api.Services
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            MimeMessage message = new MimeMessage();
            var builder = new BodyBuilder { TextBody = "", HtmlBody = htmlMessage };
            using (var client = new SmtpClient())
            {
                message.From.Add(new MailboxAddress("Veterinary", "vetproject.thesis@gmail.com"));
                message.To.Add(new MailboxAddress("Veterinary Felhasználó", email));
                message.Subject = subject;
                message.Body = builder.ToMessageBody();
                client.Connect("smtp.gmail.com", 465, SecureSocketOptions.SslOnConnect);

                client.Authenticate("vetproject.thesis@gmail.com", "htxowilrpfatmjpp");

                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }
}
