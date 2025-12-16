using MediatR;
using System;

namespace SubscriptionPlatform.Application.Features.Products.Commands
{
    public class CreateProductCommand : IRequest<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public bool IsActive { get; set; } = true;
    }
}