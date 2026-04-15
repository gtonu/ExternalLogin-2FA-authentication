using System.Diagnostics;
using System.Runtime.InteropServices;
using ExternalLoginAnd2FA.Domain.Entities;
using ExternalLoginAnd2FA.Infrastructure.Data;
using ExternalLoginAnd2FA.Web.Models;
using Microsoft.AspNetCore.Mvc;

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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
