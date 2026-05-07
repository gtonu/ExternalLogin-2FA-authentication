using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalLoginAnd2FA.Infrastructure.Middleware
{
    public class LocalizationMiddleware
    {
        private readonly RequestDelegate _next;
        public LocalizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var culture = context.GetRouteValue("culture")?.ToString();

            if(!string.IsNullOrEmpty(culture))
            {
                context.Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddMonths(1),
                    HttpOnly = true,
                    Secure = true,
                    IsEssential = true,
                    SameSite = SameSiteMode.Lax
                }
             );
            }
            

            await _next(context).ConfigureAwait(false);
        }
    }
}
