using Microsoft.EntityFrameworkCore;
using SubscriptionPlatform.Application.Interfaces.Repositories;
using SubscriptionPlatform.Domain.Entities;
using SubscriptionPlatform.Infrastructure.Persistence;

namespace SubscriptionPlatform.Infrastructure.Repositories
{
    public class InvoiceRepository : GenericRepository<Invoice>, IInvoiceRepository
    {
        public InvoiceRepository(ApplicationDbContext context) : base(context)
        {}

        public async Task<IReadOnlyList<Invoice>> GetBySubscriptionIdAsync(Guid subscriptionId)
        {
            return await _context.Invoices
                .AsNoTracking() // performans için takip etme kapatıldı
                .Where(i => i.SubscriptionId == subscriptionId)
                .ToListAsync(); 
        }     
        
    }
}