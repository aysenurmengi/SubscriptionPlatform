using Microsoft.EntityFrameworkCore;
using SubscriptionPlatform.Application.Interfaces.Repositories;
using SubscriptionPlatform.Domain.Entities;
using SubscriptionPlatform.Infrastructure.Persistence;

namespace SubscriptionPlatform.Infrastructure.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context)
        {}
        public async Task<List<Product>> SearchByTagsAsync(List<string> tags, int count)
        {
            var query = _context.Products.Where(p => p.IsActive).AsQueryable();

            if (tags != null && tags.Any())
            {
                query = query.Where(p => tags.Any(tag => p.Tags.Contains(tag)));
            }

        return await query
            .OrderBy(p => Guid.NewGuid())
            .Take(count)
            .ToListAsync();
        } 
    }
}