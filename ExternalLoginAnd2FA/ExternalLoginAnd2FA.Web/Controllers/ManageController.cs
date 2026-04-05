using ExternalLoginAnd2FA.Domain.Utilities;
using ExternalLoginAnd2FA.Infrastructure.Identity;
using ExternalLoginAnd2FA.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ExternalLoginAnd2FA.Web.Controllers
{
    public class ManageController : Controller
    {
        private readonly SignInManager<BlogSiteUser> _signInManager;
        private readonly UserManager<BlogSiteUser> _userManager;
        private readonly IUserStore<BlogSiteUser> _userStore;
        private readonly IUserEmailStore<BlogSiteUser> _emailStore;
        private readonly ILogger<AccountController> _logger;
        private readonly IEmailUtility _emailUtility;

        public ManageController(
            UserManager<BlogSiteUser> userManager,
            IUserStore<BlogSiteUser> userStore,
            SignInManager<BlogSiteUser> signInManager,
            ILogger<AccountController> logger,
            IEmailUtility emailUtility)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailUtility = emailUtility;
        }
        public async Task<IActionResult> Index(BlogSiteUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var model = new AddPhoneNumberModel();

            model.Username = userName;
            model.PhoneNumber = phoneNumber;
            return View(model);
        }

        private BlogSiteUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<BlogSiteUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(BlogSiteUser)}'. " +
                    $"Ensure that '{nameof(BlogSiteUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<BlogSiteUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<BlogSiteUser>)_userStore;
        }
    }
}
