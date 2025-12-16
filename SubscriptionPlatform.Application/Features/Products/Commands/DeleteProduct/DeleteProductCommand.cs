using MediatR;
using System;

namespace SubscriptionPlatform.Application.Features.Products.Commands
{
    public class DeleteProductCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }
}