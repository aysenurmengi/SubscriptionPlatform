using SubscriptionPlatform.Application.Interfaces.Repositories;

namespace SubscriptionPlatform.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public ICustomerRepository Customers { get; private set; } 
        public ISubscriptionRepository Subscriptions { get; private set; }
        public IProductRepository Products { get; private set; }
        public IOrderRepository Orders { get; private set; }
        public IPreferenceRepository Preferences { get; private set; }
        public IInventoryRepository Inventories { get; private set; }
        public IInvoiceRepository Invoices { get; private set; }
        public ISubscriptionPlanRepository SubscriptionPlans { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}