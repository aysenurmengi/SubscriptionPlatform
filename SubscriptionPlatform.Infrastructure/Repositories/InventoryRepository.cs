using Microsoft.EntityFrameworkCore;
using SubscriptionPlatform.Application.Interfaces.Repositories;
using SubscriptionPlatform.Domain.Entities;
using SubscriptionPlatform.Infrastructure.Persistence;

namespace SubscriptionPlatform.Infrastructure.Repositories
{
    public class InventoryRepository : GenericRepository<Inventory>, IInventoryRepository
    {
        public InventoryRepository(ApplicationDbContext context) : base(context)
        {}
        public async Task<Inventory?> GetByProductIdAsync(Guid productId)
        {
            return await _context.Inventory
                .Include(i => i.Product) // include -> ürün adı gibi verilerde gelsin diye
                .FirstOrDefaultAsync(i => i.ProductId == productId);
        }

        public async Task<IEnumerable<Inventory>> GetLowStockProductsAsync(int threshold)
        {
            return await _context.Inventory
                .Include(i => i.Product) 
                .Where(i => i.StockQuantity <= i.LowStockThreshold && !i.IsLowStockAlertSent)
                .ToListAsync();
        }
    }
}