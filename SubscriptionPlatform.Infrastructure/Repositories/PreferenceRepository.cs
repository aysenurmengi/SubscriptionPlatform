using Microsoft.EntityFrameworkCore;
using SubscriptionPlatform.Application.Interfaces.Repositories;
using SubscriptionPlatform.Domain.Entities;
using SubscriptionPlatform.Infrastructure.Persistence;

namespace SubscriptionPlatform.Infrastructure.Repositories
{
    public class PreferenceRepository: GenericRepository<Preference>, IPreferenceRepository
    {
        public PreferenceRepository(ApplicationDbContext context) : base(context){}

        public async Task AddRangeAsync(IEnumerable<Preference> entities)
        {
            await _context.Preferences.AddRangeAsync(entities);
        }

        //bellekte işlem yaptığımız için async değil
        public Task DeleteRangeAsync(IEnumerable<Preference> entities)
        {
            _context.Preferences.RemoveRange(entities);
            return Task.CompletedTask;
        }

        public async Task<IReadOnlyList<Preference>> GetAllByCustomerIdAsync(Guid customerId)
        {
            return await _context.Preferences
                .AsNoTracking()
                .Where(p => p.CustomerId == customerId)
                .ToListAsync();
        }
    }
}