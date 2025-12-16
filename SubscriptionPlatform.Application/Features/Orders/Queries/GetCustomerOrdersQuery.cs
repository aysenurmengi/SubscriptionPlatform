using MediatR;
using SubscriptionPlatform.Domain.Enums;
using System;
using System.Collections.Generic;

namespace SubscriptionPlatform.Application.Features.Orders.Queries
{
    public class GetCustomerOrdersQuery : IRequest<IReadOnlyList<OrderDto>>
    {
        public Guid CustomerId { get; set; }
    }
    public class OrderDto
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public OrderStatus Status { get; set; }
        public ShippingStatus ShippingStatus { get; set; }
        public string ShippingAddress { get; set; }
        public string TrackingNumber { get; set; }
        public bool IsSubscriptionRenewal { get; set; }
    }
}