using Microsoft.EntityFrameworkCore;
using SubscriptionPlatform.Application.Interfaces.Repositories;
using SubscriptionPlatform.Domain.Entities;
using SubscriptionPlatform.Infrastructure.Persistence;

namespace SubscriptionPlatform.Infrastructure.Repositories
{
    public class SubscriptionRepository : GenericRepository<Subscription>, ISubscriptionRepository
    {
        public SubscriptionRepository(ApplicationDbContext context) : base(context){}

        public async Task<IEnumerable<Subscription>> GetByCustomerIdAsync(Guid customerId)
        {
            return await _context.Subscriptions
                .Where(s => s.CustomerId == customerId)
                .Include(s => s.Customer)
                .ToListAsync();
        }

        public async Task<IEnumerable<Subscription>> GetRenewalsDueAsync(DateTime date)
        {
            return await _context.Subscriptions
                .Where(s => s.NextRenewalDate.Date == date.Date)
                .Include(s => s.Customer)
                .ToListAsync();
        }
    }
}