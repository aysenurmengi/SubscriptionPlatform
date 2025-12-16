using SubscriptionPlatform.Domain.Entities;

namespace SubscriptionPlatform.Application.Interfaces.Repositories
{
    public interface ISubscriptionRepository : IGenericRepository<Subscription>
    {
        Task<IEnumerable<Subscription>> GetRenewalsDueAsync(DateTime date);
        Task<IEnumerable<Subscription>> GetByCustomerIdAsync(Guid customerId); //LINQ sorgusu için
    }
}