using MediatR;

namespace SubscriptionPlatform.Application.Features.Orders.Commands.CreateOrder
{
    public class CreateOrderCommand : IRequest<Guid>
    {
        public Guid SubscriptionId { get; set; }
        public Guid CustomerId { get; set; }
        public bool IsSubscriptionRenewal { get; set; } 
        public string ShippingAddress { get; set; }
    }
}