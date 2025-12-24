using MediatR;

namespace SubscriptionPlatform.Application.Features.Products.Queries.GetAllProducts
{
    public record GetAllProductsQuery() : IRequest<List<ProductDto>>;
}