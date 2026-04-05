using DevSkill.Blog.Domain.Email;
using DevSkill.Blog.Domain.Utilities;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Blog.Infrastructure.Utilities
{
    public class EmailUtility : IEmailUtility
    {
        private SmtpSettings _smtpSettings;
        private ILogger<EmailUtility> _logger;
        public EmailUtility(IOptions<SmtpSettings> smtpSettings, ILogger<EmailUtility> logger)
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

            message.Body = new TextPart("plain")
            {
                Text = emailBody
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_smtpSettings.Host, _smtpSettings.Port,
                    _smtpSettings.SmtpEncryption != SmtpEncryptionTypes.Normal);
                client.Timeout = _smtpSettings.TimeOut;

                if (!string.IsNullOrEmpty(_smtpSettings.Username))
                {
                    try
                    {
                        await client.AuthenticateAsync(_smtpSettings.Username, _smtpSettings.Password);
                    }
                    catch (Exception ex)
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
