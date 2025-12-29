using Microsoft.EntityFrameworkCore;
using SubscriptionPlatform.Application.Interfaces.Repositories;
using SubscriptionPlatform.Domain.Entities;
using SubscriptionPlatform.Infrastructure.Persistence;

namespace SubscriptionPlatform.Infrastructure.Repositories
{
    public class SubscriptionPlanRepository : GenericRepository<SubscriptionPlan>, ISubscriptionPlanRepository
    {
        public SubscriptionPlanRepository(ApplicationDbContext context) : base(context)
        {}

        public async Task<SubscriptionPlan?> GetByIdWithItemsAsync(Guid id)
        {
            return await _context.SubscriptionPlans
                .Include(p => p.PlanItems)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}