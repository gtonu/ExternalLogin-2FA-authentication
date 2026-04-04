using ExternalLoginAnd2FA.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExternalLoginAnd2FA.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<BlogSiteUser,
        BlogSiteRole,
        Guid,
        BlogSiteUserClaim,
        BlogSiteUserRole,
        BlogSiteUserLogin,
        BlogSiteRoleClaim,
        BlogSiteUserToken>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
