using SubscriptionPlatform.Domain.Entities;

namespace SubscriptionPlatform.Application.Interfaces.Repositories
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<IEnumerable<Order>>GetPendingFulfillmentOrdersAsync();
        Task<IEnumerable<Order>> GetByCustomerIdAsync(Guid customerId);
        Task<Order?> GetLatestOrderByCustomerIdAsync(Guid customerId);
    }
}