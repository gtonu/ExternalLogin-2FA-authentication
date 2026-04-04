using ExternalLoginAnd2FA.Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System;

namespace ExternalLoginAnd2FA.Infrastructure.Identity
{
    public class BlogSiteUser : IdentityUser<Guid>,IAggregateRoot<Guid>
    {
        public override string? PhoneNumber { get => base.PhoneNumber; set => base.PhoneNumber = value; }
        public string? CountryName { get; set; }
        public string? CountryDialCode { get; set; }
        public override bool PhoneNumberConfirmed { get => base.PhoneNumberConfirmed; set => base.PhoneNumberConfirmed = value; }
        public DateTime RegistrationDate { get; set; }

    }
}
