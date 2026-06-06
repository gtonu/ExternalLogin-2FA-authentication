using ExternalLoginAnd2FA.Infrastructure.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalLoginAnd2FA.Infrastructure.Middleware
{
    public class LocalizationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string[] _supportedCultures = ["en", "bn"];

        public LocalizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            string culture = "def";

            var segments = context.Request.Path.Value?
                .Split('/', StringSplitOptions.RemoveEmptyEntries);

            if (segments?.Length > 0)
            {
                var routeCulture = segments[0];

                if (_supportedCultures.Contains(routeCulture))
                {
                    culture = routeCulture;
                }
            }

            if (culture == "def")
            {
                var cookie = context.Request.Cookies[
                    CookieRequestCultureProvider.DefaultCookieName];

                if (!string.IsNullOrWhiteSpace(cookie))
                {
                    var parsed =
                        CookieRequestCultureProvider.ParseCookieValue(cookie);

                    var cookieCulture =
                        parsed?.UICultures.FirstOrDefault().Value;

                    if (!string.IsNullOrWhiteSpace(cookieCulture))
                    {
                        cookieCulture = cookieCulture.Split('-')[0];

                        if (_supportedCultures.Contains(cookieCulture))
                        {
                            culture = cookieCulture;
                        }
                    }
                }
            }

            if (culture == "def")
            {
                var browserLanguage =
                    context.Request.Headers.AcceptLanguage
                        .ToString()
                        .Split(',')
                        .FirstOrDefault();

                if (!string.IsNullOrWhiteSpace(browserLanguage))
                {
                    browserLanguage = browserLanguage
                        .Split('-')[0]
                        .ToLower(CultureInfo.CurrentCulture);

                    if (_supportedCultures.Contains(browserLanguage))
                    {
                        culture = browserLanguage;
                    }
                }
            }

            culture = culture == "def"
                ? "en"
                : culture;

            var cultureInfo = new CultureInfo(culture);

            CultureInfo.CurrentCulture = cultureInfo;
            CultureInfo.CurrentUICulture = cultureInfo;

            context.Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(
                    new RequestCulture(culture)),
                CookieOptionsFactory.Create());

            await _next(context);
        }
    }
}
