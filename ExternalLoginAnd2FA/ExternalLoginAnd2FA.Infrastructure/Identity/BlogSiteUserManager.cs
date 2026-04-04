using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ExternalLoginAnd2FA.Infrastructure.Identity
{
    public class BlogSiteUserManager
        : UserManager<BlogSiteUser>
    {
        public BlogSiteUserManager(IUserStore<BlogSiteUser> store, 
            IOptions<IdentityOptions> optionsAccessor, 
            IPasswordHasher<BlogSiteUser> passwordHasher, 
            IEnumerable<IUserValidator<BlogSiteUser>> userValidators, 
            IEnumerable<IPasswordValidator<BlogSiteUser>> passwordValidators, 
            ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, 
            IServiceProvider services, ILogger<UserManager<BlogSiteUser>> logger)
            : base(store, optionsAccessor, passwordHasher, userValidators, 
                  passwordValidators, keyNormalizer, errors, services, logger)
        {
        }
    }
}
