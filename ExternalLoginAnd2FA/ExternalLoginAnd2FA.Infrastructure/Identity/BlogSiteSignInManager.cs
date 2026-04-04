using ExternalLoginAnd2FA.Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;


namespace ExternalLoginAnd2FA.Infrastructure.Identity
{
    public class BlogSiteSignInManager
        : SignInManager<BlogSiteUser>
    {
        public BlogSiteSignInManager(UserManager<BlogSiteUser> userManager, 
            IHttpContextAccessor contextAccessor, 
            IUserClaimsPrincipalFactory<BlogSiteUser> claimsFactory, 
            IOptions<IdentityOptions> optionsAccessor, 
            ILogger<SignInManager<BlogSiteUser>> logger, 
            IAuthenticationSchemeProvider schemes,
            IUserConfirmation<BlogSiteUser> userConfirmation)
            : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, userConfirmation)
        {
        }
    }
}
