using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalLoginAnd2FA.Domain.Entities
{
    public class AspNetUserSession : IAggregateRoot<Guid>
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public byte[] Value { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? ExpiresAtTime { get; set; }
        public DateTimeOffset? SlidingExpirationInSeconds { get; set; }
    }
}
