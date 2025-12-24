using MediatR;
using SubscriptionPlatform.Application.Features.Products.Queries;

namespace SubscriptionPlatform.Application.Features.Personalization.Queries.GetPersonalizedProducts
{
    public record GetPersonalizedProductsQuery(Guid CustomerId) : IRequest<List<ProductDto>>;
}