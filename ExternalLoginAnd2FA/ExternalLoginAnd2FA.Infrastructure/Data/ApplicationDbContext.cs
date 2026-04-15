using ExternalLoginAnd2FA.Domain.Entities;
using ExternalLoginAnd2FA.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

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

        public DbSet<AspNetUserSession> AspNetUserSessions { get; set; }
        public DbSet<Store> Stores { get; set; }
    }
}
