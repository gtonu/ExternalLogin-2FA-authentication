using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ExternalLoginAnd2FA.Infrastructure.Identity
{
    public class BlogSiteRoleManager
        : RoleManager<BlogSiteRole>
    {
        public BlogSiteRoleManager(IRoleStore<BlogSiteRole> store, 
            IEnumerable<IRoleValidator<BlogSiteRole>> roleValidators, 
            ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, 
            ILogger<RoleManager<BlogSiteRole>> logger)
            : base(store, roleValidators, keyNormalizer, errors, logger)
        {
        }
    }
}
