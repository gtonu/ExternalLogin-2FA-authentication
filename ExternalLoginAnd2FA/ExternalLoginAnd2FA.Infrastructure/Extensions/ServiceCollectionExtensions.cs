using ExternalLoginAnd2FA.Infrastructure.Data;
using ExternalLoginAnd2FA.Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;


namespace ExternalLoginAnd2FA.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplicationDbContext(this IServiceCollection services,
            string connectionString, Assembly migrationAssembly)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString
            , (x) => x.MigrationsAssembly(migrationAssembly)));
        }
        public static void AddModifiedIdentity(this IServiceCollection services)
        {
            services
                .AddIdentity<BlogSiteUser, BlogSiteRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddUserManager<BlogSiteUserManager>()
                .AddRoleManager<BlogSiteRoleManager>()
                .AddSignInManager<BlogSiteSignInManager>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;

                //SignIn settings.
                options.SignIn.RequireConfirmedAccount = false;
            });

            services.Configure<CookieAuthenticationOptions>(IdentityConstants.TwoFactorRememberMeScheme,options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromSeconds(30);
            });
            services.Configure<DataProtectionTokenProviderOptions>(options =>
                options.TokenLifespan = TimeSpan.FromMinutes(1));
        }
    }
}
