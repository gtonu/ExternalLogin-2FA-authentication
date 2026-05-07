using ExternalLoginAnd2FA.Domain.Entities;
using ExternalLoginAnd2FA.Infrastructure.Data;
using ExternalLoginAnd2FA.Web.Models;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace ExternalLoginAnd2FA.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _dbContext;

        public HomeController(ILogger<HomeController> logger,ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var store1 = new Store
            {
                Id = Guid.NewGuid(),
                StoreName = "Apple",
                ItemCount = 10
            };

            await _dbContext.Stores.AddAsync(store1);
            await _dbContext.SaveChangesAsync();

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            // Save the choice in the ASP.NET Core standard culture cookie
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );
            return LocalRedirect($"/{culture}");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
