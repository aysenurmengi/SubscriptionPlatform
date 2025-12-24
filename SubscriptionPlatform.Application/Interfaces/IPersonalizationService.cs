using SubscriptionPlatform.Application.DTOs.Products;

namespace SubscriptionPlatform.Application.Interfaces
{
    public interface IPersonalizationService
    {
        Task<List<RecommendedProductDto>> GetRecommendedProductsAsync(Guid customerId, int count = 3);
    }
    
} 