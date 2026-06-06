using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalLoginAnd2FA.Infrastructure.Utilities
{
    public class CookieOptionsFactory
    {
        public static CookieOptions Create()
        {
            return new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddMonths(1),
                HttpOnly = true,
                Secure = true,
                IsEssential = true,
                SameSite = SameSiteMode.Lax
            };
        }
    }
}
