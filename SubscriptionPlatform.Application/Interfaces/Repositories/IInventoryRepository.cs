using SubscriptionPlatform.Domain.Entities;

namespace SubscriptionPlatform.Application.Interfaces.Repositories
{
    public interface IInventoryRepository : IGenericRepository<Inventory>
    {
        Task<IEnumerable<Inventory>> GetLowStockProductsAsync(int threshold);
        Task<Inventory?> GetByProductIdAsync(Guid productId);
    }

}