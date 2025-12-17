using SubscriptionPlatform.Application.Interfaces.Repositories;
using SubscriptionPlatform.Domain.Entities;
using SubscriptionPlatform.Infrastructure.Persistence;

namespace SubscriptionPlatform.Infrastructure.Repositories
{
    public class SubscriptionPlanRepository : GenericRepository<SubscriptionPlan>, ISubscriptionPlanRepository
    {
        public SubscriptionPlanRepository(ApplicationDbContext context) : base(context)
        {}
    }
}