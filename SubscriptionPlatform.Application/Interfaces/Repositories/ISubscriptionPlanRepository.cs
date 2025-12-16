using SubscriptionPlatform.Domain.Entities;

namespace SubscriptionPlatform.Application.Interfaces.Repositories
{
    public interface ISubscriptionPlanRepository : IGenericRepository<SubscriptionPlan>
    {
        Task<IEnumerable<SubscriptionPlan>> GetByIdAsync();
    }
}