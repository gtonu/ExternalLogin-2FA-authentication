using ExternalLoginAnd2FA.Domain.Email;
using ExternalLoginAnd2FA.Domain.Utilities;
using MailKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;


namespace ExternalLoginAnd2FA.Infrastructure.Utilities
{
    public class HtmlEmailUtility : IEmailUtility
    {
        private SmtpSettings _smtpSettings;
        private ILogger<HtmlEmailUtility> _logger;
        public HtmlEmailUtility(IOptions<SmtpSettings> smtpSettings,ILogger<HtmlEmailUtility> logger)
        {
            _smtpSettings = smtpSettings.Value;
            _logger = logger;
        }
        public async Task SendEmailAsync(string receiverEmail, string receiverName, string emailSubject, string emailBody)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_smtpSettings.FromName, _smtpSettings.FromEmail));
            message.To.Add(new MailboxAddress(receiverName, receiverEmail));
            message.Subject = emailSubject;

            message.Body = new TextPart("html")
            {
                Text = emailBody
            };

            using (var client = new SmtpClient())
            {

                await client.ConnectAsync(_smtpSettings.Host, _smtpSettings.Port,
                    _smtpSettings.SmtpEncryption != SmtpEncryptionTypes.Normal);
                client.Timeout = _smtpSettings.TimeOut;

                if(!string.IsNullOrEmpty(_smtpSettings.Username))
                {
                    try
                    {
                        await client.AuthenticateAsync(_smtpSettings.Username, _smtpSettings.Password);
                    }
                    catch(Exception ex)
                    {
                        _logger.LogError(ex, "Failed to authenticate user credentials from email client");
                    }
                }

                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }
}
