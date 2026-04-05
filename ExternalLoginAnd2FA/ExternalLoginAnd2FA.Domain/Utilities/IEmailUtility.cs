using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Blog.Domain.Utilities
{
    public interface IEmailUtility
    {
        Task SendEmailAsync(string receiverEmail, string receiverName, string emailSubject, string emailBody);
    }
}
