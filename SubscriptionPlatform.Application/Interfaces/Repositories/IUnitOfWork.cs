using System;
using System.Threading.Tasks;

namespace SubscriptionPlatform.Application.Interfaces.Repositories
{
    // repositoryleri yönetir ve işlemi sonlandırır
    public interface IUnitOfWork : IDisposable
    {
        ICustomerRepository Customers { get; }
        ISubscriptionRepository Subscriptions { get; }
        IProductRepository Products { get; }
        IOrderRepository Orders { get; }
        IPreferenceRepository Preferences { get; }
        IInventoryRepository Inventories { get; }
        IInvoiceRepository Invoices { get; }
        ISubscriptionPlanRepository SubscriptionPlans { get; }
        IUserRepository Users { get; }
    

        // transaction kontrolü burada 
        Task<int> CompleteAsync(); 
    }
}