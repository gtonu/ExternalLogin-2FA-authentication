using ExternalLoginAnd2FA.Domain.Entities;
using ExternalLoginAnd2FA.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace ExternalLoginAnd2FA.Infrastructure.Identity
{
    public class DbSessionStore : ITicketStore
    {
        //private readonly ApplicationDbContext _dbContext;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly TicketSerializer _ticketSerializer;
        //public DbSessionStore(ApplicationDbContext dbContext)
        //{
        //    _dbContext = dbContext;
        //    _ticketSerializer = TicketSerializer.Default;
        //}
        public DbSessionStore(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            _ticketSerializer = TicketSerializer.Default;
        }
        
        public async Task RemoveAsync(string key)
        {
            var sessionKey = Guid.Empty;
            if (Guid.TryParse(key, out Guid result))
                sessionKey = result;
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var currentSession = await dbContext.AspNetUserSessions.FindAsync(sessionKey);
                if (currentSession == null)
                    return;

                dbContext.AspNetUserSessions.Remove(currentSession);
                await dbContext.SaveChangesAsync();
            }
            
        }

        public async Task RenewAsync(string key, AuthenticationTicket ticket)
        {
            var sessionKey = Guid.Empty;
            if (Guid.TryParse(key, out Guid result))
                sessionKey = result;
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var currentSession = await dbContext.AspNetUserSessions.FindAsync(sessionKey);
                if (currentSession == null)
                    return;

                currentSession.Value = SerializeTicket(ticket);
                currentSession.ExpiresAtTime = ticket.Properties.ExpiresUtc;

                await dbContext.SaveChangesAsync();
            }
                

        }

        public async Task<AuthenticationTicket?> RetrieveAsync(string key)
        {
            var sessionKey = Guid.Empty;
            if (Guid.TryParse(key, out Guid result))
                sessionKey = result;
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var currentSession = await dbContext.AspNetUserSessions.FindAsync(sessionKey);
                if (currentSession == null)
                    return null;

                var ticket = DeserializeTicket(currentSession.Value);
                return ticket;
            }
            
        }

        public async Task<string> StoreAsync(AuthenticationTicket ticket)
        {
            var userId = Guid.Empty;
            if(Guid.TryParse(ticket.Principal.FindFirstValue(ClaimTypes.NameIdentifier),out Guid result))
            {
                userId = result;
            }
            var session = new AspNetUserSession
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Value = SerializeTicket(ticket),
                CreatedAt = DateTime.UtcNow,
                ExpiresAtTime = ticket.Properties.ExpiresUtc
            };
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                await dbContext.AspNetUserSessions.AddAsync(session);
                await dbContext.SaveChangesAsync();
            }
            

            return session.Id.ToString();
        }

        private byte[] SerializeTicket(AuthenticationTicket ticket)
        {
            byte[] serializedTicket = _ticketSerializer.Serialize(ticket);
            return serializedTicket;
            
        }

        private AuthenticationTicket DeserializeTicket(byte[]? serializedTicket)
        {
            AuthenticationTicket? deserializedTicket = _ticketSerializer.Deserialize(serializedTicket);
            return deserializedTicket;
        }

        
    }
}
