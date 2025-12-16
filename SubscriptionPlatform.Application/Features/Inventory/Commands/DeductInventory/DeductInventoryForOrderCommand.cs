using MediatR;
using System;
using System.Collections.Generic;

namespace SubscriptionPlatform.Application.Features.Inventory.Commands
{
    public class DeductInventoryForOrderCommand : IRequest<Unit>
    {
        public Guid OrderId { get; set; }
        public Dictionary<Guid, int> ItemsToDeduct { get; set; } 
    }
}