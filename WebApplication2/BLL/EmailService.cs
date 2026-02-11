using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MailKit.Net.Smtp;
using MimeKit;
using WebApplication2.Models;

namespace WebApplication2.BLL
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;
        private readonly EmailSettings _emailSettings;

        public EmailService(ILogger<EmailService> logger, IOptions<EmailSettings> emailSettings)
        {
            _logger = logger;
            _emailSettings = emailSettings.Value;
        }

        public async Task SendWinnerNotificationAsync(string email, string userName, string giftName)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));
                message.To.Add(new MailboxAddress(userName, email));
                message.Subject = "מזל טוב! זכית בהגרלה!";

                message.Body = new TextPart("html")
                {
                    Text = $@"
                        <div dir='rtl' style='font-family: Arial, sans-serif;'>
                            <h2>שלום {userName},</h2>
                            <p><strong>מזל טוב! זכית בהגרלה!</strong></p>
                            <p>אנו שמחים לבשר לך שזכית במתנה: <strong>{giftName}</strong></p>
                            <p>אנא צור קשר איתנו לתיאום איסוף המתנה.</p>
                            <p>ברכות,<br/>צוות מערכת ההגרלות</p>
                        </div>"
                };

                using var client = new SmtpClient();
                await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                
                await client.AuthenticateAsync(_emailSettings.SenderEmail, _emailSettings.SenderPassword);
                
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "שגיאה בשליחת מייל זכייה ל-{Email}", email);
                throw;
            }
        }
    }
}