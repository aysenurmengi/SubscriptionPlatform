using AutoMapper;
using MediatR;
using SubscriptionPlatform.Application.Interfaces.Repositories;
using SubscriptionPlatform.Application.Features.Products.Queries;

namespace SubscriptionPlatform.Application.Features.Personalization.Queries.GetPersonalizedProducts
{
    public class GetPersonalizedProductsQueryHandler : IRequestHandler<GetPersonalizedProductsQuery, List<ProductDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetPersonalizedProductsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<ProductDto>> Handle(GetPersonalizedProductsQuery request, CancellationToken cancellationToken)
        {
            var preferences = await _unitOfWork.Preferences.GetAllByCustomerIdAsync(request.CustomerId);
            var allProductsList = await _unitOfWork.Products.GetAllAsync();

            if (preferences == null || !preferences.Any())
            {
                return _mapper.Map<List<ProductDto>>(allProductsList.Take(10).ToList());
            }

            var filteredProducts = allProductsList.Select(product => new
            {
                Product = product,
                MatchCount = preferences.Count(pref =>
                {
                    if (string.IsNullOrWhiteSpace(product.Tags) || string.IsNullOrWhiteSpace(pref.Value))
                        return false;

                    return product.Tags.Contains(pref.Value.Trim(), StringComparison.OrdinalIgnoreCase);
                })
            })
            .Where(x => x.MatchCount > 0)
            .OrderByDescending(x => x.MatchCount)
            .Select(x => x.Product)
            .ToList();

            if (!filteredProducts.Any())
            {
                return _mapper.Map<List<ProductDto>>(allProductsList.Take(5).ToList());
            }

            return _mapper.Map<List<ProductDto>>(filteredProducts);
        }
    }
}