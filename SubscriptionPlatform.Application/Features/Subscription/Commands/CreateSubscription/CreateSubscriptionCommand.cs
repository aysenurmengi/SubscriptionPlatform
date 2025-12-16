using MediatR;
using System;
using SubscriptionPlatform.Domain.Enums;

namespace SubscriptionPlatform.Application.Features.Subscriptions.Commands
{
    public class CreateSubscriptionCommand : IRequest<Guid>
    {
        public Guid CustomerId { get; set; }
        public Guid PlanId { get; set; }
        public int Quantity { get; set; } = 1;
        public string CardToken { get; set; } 
        public BillingCycle Cycle { get; set; }
        
        public string ShippingAddress { get; set; }
    }
}