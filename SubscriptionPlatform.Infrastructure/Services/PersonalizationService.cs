using AutoMapper;
using SubscriptionPlatform.Application.DTOs.Products;
using SubscriptionPlatform.Application.Interfaces;
using SubscriptionPlatform.Application.Interfaces.Repositories;
using SubscriptionPlatform.Infrastructure.Persistence;

namespace SubscriptionPlatform.Infrastructure.Services
{
    public class PersonalizationService : IPersonalizationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public PersonalizationService(UnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        
        public async Task<List<RecommendedProductDto>> GetRecommendedProductsAsync(Guid customerId, int count = 3)
        {
            var preferences = await _unitOfWork.Preferences.GetAllByCustomerIdAsync(customerId);
            
            if (preferences == null || !preferences.Any())
                return new List<RecommendedProductDto>();

            var searchTerms = preferences.Select(p => p.Value.ToLower().Trim()).ToList();
            var matchedProducts = await _unitOfWork.Products.SearchByTagsAsync(searchTerms, count);

            return _mapper.Map<List<RecommendedProductDto>>(matchedProducts);
        }
    }
}