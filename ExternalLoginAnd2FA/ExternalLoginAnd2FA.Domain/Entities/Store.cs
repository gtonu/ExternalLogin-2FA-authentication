using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalLoginAnd2FA.Domain.Entities
{
    public class Store : IAggregateRoot<Guid>
    {
        public Guid Id { get; set; }
        public string StoreName { get; set; }
        public int ItemCount { get; set; }
    }
}
