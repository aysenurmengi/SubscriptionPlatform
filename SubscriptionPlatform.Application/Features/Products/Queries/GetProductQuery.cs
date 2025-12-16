using MediatR;
using System;

namespace SubscriptionPlatform.Application.Features.Products.Queries
{
    public class GetProductQuery : IRequest<ProductDto>
    {
        public Guid Id { get; set; }
    }

    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public bool IsActive { get; set; }
    }
}