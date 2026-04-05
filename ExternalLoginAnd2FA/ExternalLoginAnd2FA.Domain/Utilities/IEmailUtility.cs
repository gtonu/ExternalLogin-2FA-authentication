using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalLoginAnd2FA.Domain.Utilities
{
    public interface IEmailUtility
    {
        Task SendEmailAsync(string receiverEmail, string receiverName, string emailSubject, string emailBody);
    }
}
