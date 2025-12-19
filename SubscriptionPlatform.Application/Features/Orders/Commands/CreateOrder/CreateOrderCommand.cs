using MediatR;

namespace SubscriptionPlatform.Application.Features.Orders.Commands.CreateOrder
{
    public class CreateOrderCommand : IRequest<Guid>
    {
        public Guid CustomerId { get; set; }
        public string ShippingAddress { get; set; }
        public Guid? SubscriptionId { get; set; } // abonelik değilse null
        public bool IsSubscriptionRenewal { get; set; } = false;
        public List<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
    }

    public class OrderItemDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}