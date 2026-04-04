using System;

using Microsoft.AspNetCore.Identity;

namespace ExternalLoginAnd2FA.Infrastructure.Identity
{
    public class BlogSiteUserToken
        : IdentityUserToken<Guid>
    {
        public override Guid UserId { get => base.UserId; set => base.UserId = value; }
        public override string LoginProvider { get => base.LoginProvider; set => base.LoginProvider = value; }
        public override string Name { get => base.Name; set => base.Name = value; }
        public override string? Value { get => base.Value; set => base.Value = value; }
    }
}
