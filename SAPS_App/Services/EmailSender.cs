using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SAPS_App.Models;

namespace SAPS_App.Services
{
    public class EmailSender: IEmailSender
    {
        private readonly IOptions<SmtpSettings> smtpSetting;

        public EmailSender(IOptions<SmtpSettings> smtpSetting)
        {
            this.smtpSetting = smtpSetting;
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var message = new MailMessage
            {
                From = new MailAddress(smtpSetting.Value.User),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true
            };
            message.To.Add(email);
            using (var emailClient = new SmtpClient(smtpSetting.Value.Host, smtpSetting.Value.Port))
            {
                emailClient.Credentials = new NetworkCredential(
                    smtpSetting.Value.User,
                    smtpSetting.Value.Password
                );
                emailClient.EnableSsl = smtpSetting.Value.EnableSSL; // Enable SSL if required
                await emailClient.SendMailAsync(message);
            }
        }
    }
}
