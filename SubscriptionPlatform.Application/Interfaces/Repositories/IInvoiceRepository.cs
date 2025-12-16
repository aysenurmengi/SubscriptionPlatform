using SubscriptionPlatform.Domain.Entities;

namespace SubscriptionPlatform.Application.Interfaces.Repositories
{
    public interface IInvoiceRepository : IGenericRepository<Invoice>
    {
        Task<IReadOnlyList<Invoice>> GetBySubscriptionIdAsync(Guid subscriptionId);
    }
}