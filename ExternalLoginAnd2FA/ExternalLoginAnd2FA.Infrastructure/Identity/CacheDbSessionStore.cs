using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;


namespace ExternalLoginAnd2FA.Infrastructure.Identity
{
    public class CacheDbSessionStore : ITicketStore
    {
        private readonly IDistributedCache _cache;
        private readonly TicketSerializer _ticketSerializer;
        private readonly ConnectionMultiplexer _connectionMultiplexer;
        public CacheDbSessionStore(IDistributedCache cache)
        {
            _cache = cache;
            _ticketSerializer = TicketSerializer.Default;
            _connectionMultiplexer = ConnectionMultiplexer.Connect("localhost:6379");
        }
        public async Task RemoveAsync(string key)
        {
            var dbContext = _connectionMultiplexer.GetDatabase();
            
            await dbContext.StringDeleteAsync(key,When.Exists);
        }

        public async Task RenewAsync(string key, AuthenticationTicket ticket)
        {
            var dbContext = _connectionMultiplexer.GetDatabase();
            var value = await dbContext.StringGetAsync(key);
            if (value.IsNull)
                return;

            await dbContext.StringSetAsync(key, Serialize(ticket), TimeSpan.FromMinutes(5));
        }

        public async Task<AuthenticationTicket?> RetrieveAsync(string key)
        {
            var dbContext = _connectionMultiplexer.GetDatabase();
            var value = await dbContext.StringGetAsync(key);
            if (value.IsNull)
                return null;

            var deSerializedValue = Deserialize(value);
            return deSerializedValue;
        }

        public async Task<string> StoreAsync(AuthenticationTicket ticket)
        {
            var sessionKey = Guid.NewGuid().ToString();
            var dbContext = _connectionMultiplexer.GetDatabase();
            var value = Serialize(ticket);
            await dbContext.StringSetAsync(sessionKey, value, TimeSpan.FromMinutes(5));

            return sessionKey;
        }

        private byte[] Serialize(AuthenticationTicket ticket)
        {
            return _ticketSerializer.Serialize(ticket);
        }

        private AuthenticationTicket Deserialize(byte[] serializedTicket)
        {
            return _ticketSerializer.Deserialize(serializedTicket);
        }
    }
}
