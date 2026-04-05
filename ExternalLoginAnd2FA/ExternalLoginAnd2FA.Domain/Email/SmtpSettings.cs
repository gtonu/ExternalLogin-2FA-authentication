

namespace ExternalLoginAnd2FA.Domain.Email
{
    public class SmtpSettings
    {
        public string Host { get; set; } = null!;
        public int Port { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string FromName { get; set; } = null!;
        public string FromEmail { get; set; } = null!;
        public SmtpEncryptionTypes SmtpEncryption { get; set; }
        public int TimeOut { get; set; }
    }
}
