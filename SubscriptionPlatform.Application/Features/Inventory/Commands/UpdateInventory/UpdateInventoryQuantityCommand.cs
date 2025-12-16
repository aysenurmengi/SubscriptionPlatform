using MediatR;
using System;

namespace SubscriptionPlatform.Application.Features.Inventory.Commands
{
    public class UpdateInventoryQuantityCommand : IRequest<Unit>
    {
        public Guid ProductId { get; set; }
        public int NewStockQuantity { get; set; } 
        public string Reason { get; set; } 
    }
}